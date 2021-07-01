using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TriviaQuestion
{
    public string question;
    public string answer;
}

public class UITriviaManager : MonoBehaviour
{
    public static UITriviaManager instance;

    [Header("Score Attribute")]
    public int score;
    public bool answer;

    [Header("Panel Attribute")]
    public GameObject firstTimePanel;
    public GameObject gameOverPanel;
    public float startingTime = 5f;
    public float questionCountDown = 5f;
    public bool firstTime = true;

    [Header("UI Attribute")]
    public Text firstTimerText;
    public Text timerText;
    public Text questionText;
    public Text questionTotalText;
    public Text scoreTotalText;
    public Text scoreFinalText;

    [Header("Question Attribute")]
    public int questionTemp;
    public string questionFix;
    public string answerFix;
    public List<TriviaQuestion> trivia;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
            firstTimerText.text = "GET READY!\n" + startingTime.ToString("0");
            startingTime -= 1 * Time.deltaTime;

            if (startingTime < 0)
            {
                firstTimePanel.SetActive(false);
                firstTime = false;

                if (Client.instance.isHost)
                {
                    ClientSend.TriviaRequest();
                }
            }
        }

        if (trivia.Count == 0)
        {
            gameOverPanel.SetActive(true);
            scoreFinalText.text = $"Your Score's: {score}";
        }

        timerText.text = $"{questionCountDown}";
        questionTotalText.text = $"{trivia.Count}";
        scoreTotalText.text = $"{score}";
    }

    public void BackToMainMenu(string scene)
    {
        if (!Client.instance.isHost)
        {
            ClientSend.LeaveRoomRequest(RoomDatabase.instance.roomCode, Client.instance.myUname);
            RoomDatabase.instance.RemoveDatabase();
        }
        else
        {
            ClientSend.DestroyRoomRequest(RoomDatabase.instance.roomCode);
            SceneManager.LoadScene(scene);
            string uname = GameObject.Find("ClientManager").GetComponent<Client>().myUname;
            ClientSend.UpScore(uname,score);
            Debug.Log("Room Was Destroyed Successfully");
        }

        Client.instance.isPlay = false;
    }

    public void SetQuestion(int questionResult)
    {
        questionTemp = questionResult;
        questionText.text = $"{trivia[questionResult].question}";
        questionFix = trivia[questionResult].question;
        answerFix = trivia[questionResult].answer;
        StartCoroutine(QuestionCountDown());
    }

    IEnumerator QuestionCountDown()
    {
        questionCountDown -= 1;
        yield return new WaitForSeconds(1);

        if (questionCountDown > 0)
            StartCoroutine(QuestionCountDown());
        else
        {
            trivia.RemoveAt(questionTemp);
            questionCountDown = 5f;
            questionTemp = 0;

            if (answer)
            {
                score++;
                answer = false;
            }

            if (Client.instance.isHost)
                ClientSend.TriviaRequest();
        }
    }
}
