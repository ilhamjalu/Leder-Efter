# Leder-Efter
1. (Author) 4210181002 Farhan Muhammad
2. (Author) 4210181010 Ilham Jalu Prakosa
3. (Course) Praktikum Desain Game Multiplayer Online

### Game-Flow
1. Saat akan login, pemain diharuskan untuk memasukkan username dan password
2. Apabila pemain belum memilikinya, maka harus sign up
3. Kemudian, pemain akan masuk ke main menu
4. Di main menu, hanya akan terdapat play, history dan quit
5. Play: Apabila pemain memilih play, pemain akan langsung masuk ke dalam lobby
6. History: Dalam history, pemain dapat melihat 3 hasil permainan terakhir yang sudah dilakukan
7. Apabila pemain telah menyelesaikan permainan, ia akan diarahkan ke main menu

### Lobby-System
1. Kapasitas pemain sebanyak 2-30 orang
2. Dalam lobby, pemain akan diberikan 6 spot dengan warna yang berbeda (merah, hijau, biru, ungu, coklat, kuning)
3. 6 spot warna tersebut akan menjadi 6 tim dengan kapasitas pemain maksimal 5 pemain (6x5 = 30)
4. Setiap pemain akan secara bebas memasuki kelompok dengan warna apa saja
5. Apabila ada tim yang sudah berisi 5 pemain, maka setiap pemain tidak akan bisa memasuki tim tersebut
6. Permainan akan dimulai secara otomatis setelah semua pemain sudah memasuki salah satu tim

### Game-Play
1. Setiap tim akan dispawn dalam satu arena besar pada posisi yang berbeda
2. Setelah setiap tim sudah dispawn, sejumlah item juga dispawn pada posisi yang diacak
3. Tugas setiap tim yaitu mengumpulkan item yang tersebar secara acak sebanyak mungkin
4. Apabila pemain sudah berhasil mengambil satu item, maka ia harus membawanya ke posisi awal timnya
5. Pemain tidak bisa merebut item yang sudah diambil oleh pemain lain
6. Total skor dalam satu tim akan dihitung berdasarkan total item yang berhasil ditemukan oleh setiap pemain dalam tim tersebut
7. Permainan akan berakhir ketika seluruh item sudah ditemukan
