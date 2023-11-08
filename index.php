<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Data Buku</title>
    <link rel="stylesheet" href="bootstrap-5.1.3-dist\css\bootstrap.css">
    <link rel="stylesheet" href="fontawesome-free-6.0.0-web\css\all.min.css">
    
    <style type="text/css">
        .wrapper{
            width: 650px;
            margin: 0 auto;
        }
        .page-header h2{
            margin-top: 0;
        }
        table tr td:last-child a{
            margin-right: 15px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function(){
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
</head>
<body>
    <div class="wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="page-header clearfix">
                        <h2 class="pull-left">Informasi Pegawai</h2>
                        <a href="create.php" class="btn btn-success pull-right">Tambah Baru</a>
                    </div>
                    <?php
                    // Include config file
                    require_once "config.php";

                    // Attempt select query execution
                    $sql = "SELECT * FROM databuku";
                    if($result = mysqli_query($link, $sql)){
                        if(mysqli_num_rows($result) > 0){
                            echo "<table class='table table-bordered table-striped' style='width: 120%; text-align: center'>";
                                echo "<thead>";
                                    echo "<tr>";
                                        echo "<th>No</th>";
                                        echo "<th>Judul Buku</th>";
                                        echo "<th>Pengarang</th>";
                                        echo "<th>Tahun Terbit</th>";
                                        echo "<th>Jumlah Halaman</th>";
                                        echo "<th>Penerbit</th>";
                                        echo "<th>Pengaturan</th>";
                                    echo "</tr>";
                                echo "</thead>";
                                echo "<tbody>";
                                while($row = mysqli_fetch_array($result)){
                                    echo "<tr>";
                                        echo "<td>" . $row['no'] . "</td>";
                                        echo "<td>" . $row['judulbuku'] . "</td>";
                                        echo "<td>" . $row['pengarang'] . "</td>";
                                        echo "<td>" . $row['tahunterbit'] . "</td>";
                                        echo "<td>" . $row['jlh_halaman'] . "</td>";
                                        echo "<td>" . $row['tahunterbit'] . "</td>";
                                        echo "<td>";
                                            echo "<a href='read.php?id=". $row['no'] ."' title='View Record' data-toggle='tooltip'><i class='fas fa-eye'></i></a>";
                                            echo "<a href='update.php?id=". $row['no'] ."' title='Update Record' data-toggle='tooltip'><i class='fas fa-pencil-alt'></i></a>";
                                            echo "<a href='delete.php?id=". $row['no'] ."' title='Delete Record' data-toggle='tooltip'><i class='fas fa-trash-alt'></i></a>";
                                        echo "</td>";
                                    echo "</tr>";
                                }
                                echo "</tbody>";
                            echo "</table>";
                            // Free result set
                            mysqli_free_result($result);
                        } else{
                            echo "<p class='lead'><em>No records were found.</em></p>";
                        }
                    } else{
                        echo "ERROR: Could not able to execute $sql. " . mysqli_error($link);
                    }

                    // Close connection
                    mysqli_close($link);
                    ?>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

