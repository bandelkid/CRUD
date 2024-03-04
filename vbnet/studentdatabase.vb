Imports System.Data.Odbc
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar


Public Class Form1
    Dim Conn As OdbcConnection
    Dim Cmd As OdbcCommand
    Dim Ds As DataSet
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String
    'Membuat Koneksi
    Sub Koneksi()
        ' Memanggil database yaitu nama database kita adalah kampus
        MyDB = "Driver={MySQL ODBC 8.3 ANSI Driver};Database=sekolah;Server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub
    Sub KondisiAwal()
        ' Textbox1, textbox2, textbox3, textbox4 kita kosongkan pertama kali
        txtNisn.Text = ""
        txtNama.Text = ""
        txtAlamat.Text = ""
        txtJurusan.Text = ""
        txtKelas.Text = ""
        txtSearch.Text = ""

        ' Textbox1, textbox2, textbox3, textbox4.enabled = false artinya formnya kita matikan  
        txtNisn.Enabled = False
        txtNama.Enabled = False
        txtAlamat.Enabled = False
        txtJurusan.Enabled = False
        txtKelas.Enabled = False

        ' Textbox1 yang berarti form nim kita maksimalkan hanya bisa 15 huruf/angka
        txtNisn.MaxLength = 5

        ' Button1, Button2, Button3, Button4 kita tambahkan text masing - masing yaitu input, edit, hapus, tutup
        btnSave.Text = "INPUT"
        btnEdit.Text = "EDIT"
        btnDelete.Text = "HAPUS"
        btnClear.Text = "TUTUP"

        ' Button1, Button2, Button3, Button4 kita aktifkan dengan menggunakan perintah true
        btnSave.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnClear.Enabled = True

        ' Panggil koneksi yang sudah kita buat sub Koneksi()
        Call Koneksi()

        ' Memanggil table yang sudah kita buat yaitu mahasiswa
        Da = New OdbcDataAdapter("Select * From murid", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "murid")
        DataGridView1.DataSource = Ds.Tables("murid")
    End Sub
    Sub FieldAktif()
        ' untuk mengaktifkan form
        txtNisn.Enabled = True
        txtNama.Enabled = True
        txtAlamat.Enabled = True
        txtJurusan.Enabled = True
        txtKelas.Enabled = True
        txtSearch.Enabled = True
        txtNisn.Focus()
    End Sub
    Private selectedRowIndex As Integer = -1
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            selectedRowIndex = e.RowIndex

            ' Mengisi TextBox dengan nilai dari baris yang dipilih
            txtNisn.Text = DataGridView1.Rows(selectedRowIndex).Cells("nisn").Value.ToString()
            txtNama.Text = DataGridView1.Rows(selectedRowIndex).Cells("nama").Value.ToString()
            txtAlamat.Text = DataGridView1.Rows(selectedRowIndex).Cells("alamat").Value.ToString()
            txtJurusan.Text = DataGridView1.Rows(selectedRowIndex).Cells("jurusan").Value.ToString()
            txtKelas.Text = DataGridView1.Rows(selectedRowIndex).Cells("kelas").Value.ToString()
        End If
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        ' if button1.text yang textnya "input" maka akan berubah menjadi button "simpan"
        If btnSave.Text = "INPUT" Then
            btnSave.Text = "SIMPAN"
            ' lalu button2 dan button3 tidak aktif dan button4 kita ubah menjadi tulisan "batal"
            btnEdit.Enabled = False
            btnDelete.Enabled = False
            btnClear.Text = "BATAL"
            ' lalu kita panggil FieldAktif() yang mana textbox1,textbox2,textbox3,textbox4 akan diaktifkan
            Call FieldAktif()
        Else
            ' if textbox1, textbox2, textbox3 dan textbox3 kosong maka muncul alert pastikan semua field terisi
            ' ini artinya disebut validasi
            If txtNisn.Text = "" Or
                txtNama.Text = "" Or
                txtAlamat.Text = "" Or
                txtJurusan.Text = "" Or
                txtKelas.Text = "" Then
                MsgBox("Pastikan semua field terisi")
            Else
                ' Jika semua form terisi, maka kita panggil Koneksi() ke database
                Call Koneksi()
                ' lalu kita masukan data kita ke table mahasiswa (insert into mahasiswa .....)
                Dim InputData As String = "Insert into murid values(
                '" & txtNisn.Text & "',
                '" & txtNama.Text & "',
                '" & txtAlamat.Text & "',
                '" & txtJurusan.Text & "',
                '" & txtKelas.Text & "')"
                Cmd = New OdbcCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                ' lalu kita tampilkan message "input data berhasil"
                MsgBox("Input data berhasil")
                ' lalu kita kembalikan ke KondisiAwal()
                Call KondisiAwal()
            End If
        End If

    End Sub


    Private Sub txtNisn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNisn.KeyPress
        If e.KeyChar = Chr(13) Then
            ' setelah tekan entar maka Koneksi() akan terpanggil
            Call Koneksi()

            Cmd = New OdbcCommand("Select * from murid where nisn='" & txtNisn.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            ' if kita panggil nim lalu tekan enter maka otomatis textbox2, textbox3, dan textbox4 akan terisi secara otomatis
            If Rd.HasRows Then
                txtNama.Text = Rd.Item("nama")
                txtAlamat.Text = Rd.Item("alamat")
                txtJurusan.Text = Rd.Item("jurusan")
                txtKelas.Text = Rd.Item("kelas")
            Else
                ' Jika nim yang kita ketikan salah, maka akan menampilkan alert atau message ("data tidak ada")
                MsgBox("Data Tidak Ada")
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If btnEdit.Text = "EDIT" Then
            btnEdit.Text = "UPDATE"
            'button1 dan button2 akan kita false artinya tidak berfungsi
            btnSave.Enabled = False
            btnDelete.Enabled = False
            'button4 kita ganti tulisan menjadi batal
            btnClear.Text = "BATAL"
            ' Lalu kita panggil FieldAktif() yang mana textbox1, textbox2, textbox3 dan textbox 4 kita aktifkan
            Call FieldAktif()
        Else
            ' ini adalah validasi jika textbox1, textbox2, textbox3 dan textbox 4 tidak terisi maka akan muncul alert ("pastikan semua terisi)
            If txtNisn.Text = "" Or
                txtNama.Text = "" Or
                txtAlamat.Text = "" Or
                txtJurusan.Text = "" Or
                txtKelas.Text = "" Then
                MsgBox("Pastikan semua field terisi")
            Else
                ' jika semua terisi panggil Koneksi()
                Call Koneksi()
                ' kita update table mahasiswa
                Dim EditData As String =
                    "Update murid set 
                    nama= '" & txtNama.Text & "',
                    alamat='" & txtAlamat.Text & "',
                    jurusan='" & txtJurusan.Text & "', 
                    kelas='" & txtKelas.Text & "' 
                    where 
                    nisn='" & txtNisn.Text & "'"
                Cmd = New OdbcCommand(EditData, Conn)
                Cmd.ExecuteNonQuery()
                ' jika berhasil tampilkan alert / message ("edit data berhasil")
                MsgBox("Edit data berhasil")
                ' setelah semua sudah tolong tampilkan KondisiAwal()
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' Jika button3.text yang mana textnya adalah hapus maka kita ubah texttnya menjadi delete  
        If btnDelete.Text = "HAPUS" Then
            btnDelete.Text = "DELETE"
            ' button1 non aktifkan
            btnSave.Enabled = False
            ' button2 juga kita non aktifkan, jadi dia ga bisa di klik sama sekali ketika button hapus kita klik
            btnEdit.Enabled = False
            ' button4.text kita ubah textnya dari tutup menjadi batal
            btnClear.Text = "BATAL"
            ' setelah itu kita aktifkan FieldAktif() yang mana artinya kita mengaktifkan textbox1, textbox2, textbox3 dan textbox4
            Call FieldAktif()
        Else
            ' Ini adalah validasi
            ' jika field tidak terisi maka tidak akan bisa di hapus
            If txtNisn.Text = "" Or
                txtNama.Text = "" Or
                txtAlamat.Text = "" Or
                txtJurusan.Text = "" Or
                txtKelas.Text = "" Then
                MsgBox("Pastikan data yang akan dihapus terisi")
            Else
                ' jika sudah kita isi fieldnya maka bisa kita hapus, prosesnya adalah
                ' kita panggil Koneksi()
                Call Koneksi()
                ' lalu kita panggil table mahasiswa lalu dia bilang "tolongin aku dong, aku mau hapus dengan nim xxx tolong di bantu ya. makasih:)"
                Dim HapusData As String = "delete from murid where nisn='" & txtNisn.Text & "'"
                Cmd = New OdbcCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                ' kalau berhasil kita tampilkan alert / message dengan tulisan "hapus data berhasil"
                MsgBox("Hapus data berhasil")
                ' lalu kita panggil kondisiAwal()
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If btnClear.Text = "TUTUP" Then
            End
            ' namun kalau tulisannya BATAL maka kita panggil KondisiAwal()
        Else
            Call KondisiAwal()
        End If
    End Sub

       Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' Pastikan textbox untuk pencarian tidak kosong
        If txtSearch.Text <> "" Then
            ' Panggil metode Koneksi untuk membuka koneksi ke database
            Call Koneksi()

            ' Buat query SQL untuk mencari data berdasarkan NISN
            Dim queryCari As String = "SELECT * FROM murid WHERE 
               nisn LIKE '%" & txtSearch.Text & "%' 
               OR nama LIKE '%" & txtSearch.Text & "%' 
               OR jurusan LIKE '%" & txtSearch.Text & "%' 
               OR kelas LIKE '%" & txtSearch.Text & "%'
               OR alamat LIKE '%" & txtSearch.Text & "%'
               "
            ' Buat adapter untuk mengambil data dari database
            Dim adapter As New OdbcDataAdapter(queryCari, Conn)

            ' Buat dataset untuk menampung hasil pencarian
            Dim ds As New DataSet()

            ' Isi dataset dengan hasil query
            adapter.Fill(ds, "murid")

            ' Bind dataset ke DataGridView
            DataGridView1.DataSource = ds.Tables("murid")

            ' Matikan tombol Update jika pencarian tidak menghasilkan data
            btnEdit.Enabled = False

            ' Buat command untuk menjalankan query
            Cmd = New OdbcCommand(queryCari, Conn)

            ' Eksekusi query dan baca hasilnya
            Dim reader As OdbcDataReader = Cmd.ExecuteReader()

            ' Periksa apakah data ditemukan berdasarkan NISN
            If reader.Read() Then
                ' Jika data ditemukan, isi nilai textbox dengan data yang ditemukan
                txtNisn.Text = reader("nisn").ToString()
                txtNama.Text = reader("nama").ToString()
                txtAlamat.Text = reader("alamat").ToString()
                txtJurusan.Text = reader("jurusan").ToString()
                txtKelas.Text = reader("kelas").ToString()

                ' Setelah data ditemukan, tampilkan tombol Update
                btnEdit.Enabled = True
                btnEdit.Text = "UPDATE"

                ' Matikan tombol Simpan, Hapus, dan Batal
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnClear.Text = "BATAL"

                ' Aktifkan textbox untuk mengedit data
                Call FieldAktif()
            End If

            ' Tutup koneksi dan bersihkan resource
            reader.Close()
            Call TutupKoneksi()
        Else
            ' Jika textbox pencarian kosong, tampilkan pesan alert
            MsgBox("Masukkan data untuk melakukan pencarian")
        End If
    End Sub


    ' Fungsi untuk menutup koneksi ke database
    Private Sub TutupKoneksi()
        ' Pastikan koneksi dalam keadaan terbuka sebelum menutupnya
        If Conn.State = ConnectionState.Open Then
            Conn.Close()

        End If
    End Sub

End Class
