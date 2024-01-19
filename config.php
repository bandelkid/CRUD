<?php
//variabel inisialisasi
$servername= "localhost";
$username = "root";
$password = "";
$database= "dbperpus";

//create connection
$link = mysqli_connect($servername, $username, $password, $database);

// Check connection
if (!$link) {
    die("KONEKSI GAGAL: " .mysqli_connect_error());
  }
 
?>
