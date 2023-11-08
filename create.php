<?php
// Include config file
require_once "config.php";

// Define variables and initialize with empty values
$judulbuku = $pengarang = $tahunterbit = $jlh_halaman = $penerbit = "";
$judulbuku_err = $pengarang_err = $tahunterbit_err = $jlh_halaman_err = $penerbit_err = "";

// Processing form data when the form is submitted
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Validate Judul Buku
    $input_judulbuku = trim($_POST["judulbuku"]);
    if (empty($input_judulbuku)) {
        $judulbuku_err = "Masukkan Judul Buku.";
    } else {
        $judulbuku = $input_judulbuku;
    }

    // Validate Pengarang
    $input_pengarang = trim($_POST["pengarang"]);
    if (empty($input_pengarang)) {
        $pengarang_err = "Masukkan Nama Pengarang.";
    } else {
        $pengarang = $input_pengarang;
    }

    // Validate Tahun Terbit
    $input_tahunterbit = trim($_POST["tahunterbit"]);
    if (empty($input_tahunterbit)) {
        $tahunterbit_err = "Masukkan Tahun Terbit.";
    } else {
        $tahunterbit = $input_tahunterbit;
    }

    // Validate Jumlah Halaman
    $input_jlh_halaman = trim($_POST["jlh_halaman"]);
    if (empty($input_jlh_halaman)) {
        $jlh_halaman_err = "Masukkan Jumlah Halaman.";
    } else {
        $jlh_halaman = $input_jlh_halaman;
    }

    // Validate Penerbit
    $input_penerbit = trim($_POST["penerbit"]);
    if (empty($input_penerbit)) {
        $penerbit_err = "Masukkan Nama Penerbit.";
    } else {
        $penerbit = $input_penerbit;
    }

    // Check input errors before inserting into the database
    if (empty($judulbuku_err) && empty($pengarang_err) && empty($tahunterbit_err) && empty($jlh_halaman_err) && empty($penerbit_err)) {
        // Prepare an insert statement
        $sql = "INSERT INTO databuku (judulbuku, pengarang, tahunterbit, jlh_halaman, penerbit) VALUES (?, ?, ?, ?, ?)";

        if ($stmt = mysqli_prepare($link, $sql)) {
            // Bind variables to the prepared statement as parameters
            mysqli_stmt_bind_param($stmt, "sssis", $param_judulbuku, $param_pengarang, $param_tahunterbit, $param_jlh_halaman, $param_penerbit);

            // Set parameters
            $param_judulbuku = $judulbuku;
            $param_pengarang = $pengarang;
            $param_tahunterbit = $tahunterbit;
            $param_jlh_halaman = $jlh_halaman;
            $param_penerbit = $penerbit;

            // Attempt to execute the prepared statement
            if (mysqli_stmt_execute($stmt)) {
                // Records created successfully. Redirect to the landing page
                header("location: index.php");
                exit();
            } else {
                echo "Something went wrong. Please try again later.";
            }
        }

        // Close statement
        mysqli_stmt_close($stmt);
    }

    // Close connection
    mysqli_close($link);
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Create Record</title>
    <link rel="stylesheet" href="bootstrap-5.1.3-dist/css/bootstrap.css">
    <link rel="stylesheet" href="fontawesome-free-6.0.0-web/css/all.min.css">
    
    <style type="text/css">
        .wrapper {
            width: 500px;
            margin: 0 auto;
        }
    </style>
</head>
<body>
    <div class="wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="page-header">
                        <h2>Tambah Record</h2>
                    </div>
                    <p>Silahkan isi form di bawah ini kemudian submit untuk menambahkan data pegawai ke dalam database.</p>
                    <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
                        <div class="form-group <?php echo (!empty($judulbuku_err)) ? 'has-error' : ''; ?>">
                            <label>Judul Buku</label>
                            <input type="text" name="judulbuku" class="form-control" value="<?php echo $judulbuku; ?>">
                            <span class="help-block"><?php echo $judulbuku_err;?></span>
                        </div>
                        <div class="form-group <?php echo (!empty($pengarang_err)) ? 'has-error' : ''; ?>">
                            <label>Pengarang</label>
                            <input type="text" name="pengarang" class="form-control" value="<?php echo $pengarang; ?>">
                            <span class="help-block"><?php echo $pengarang_err;?></span>
                        </div>
                        <div class="form-group <?php echo (!empty($tahunterbit_err)) ? 'has-error' : ''; ?>">
                            <label>Tahun Terbit</label>
                            <input type="text" name="tahunterbit" class="form-control" value="<?php echo $tahunterbit; ?>">
                            <span class="help-block"><?php echo $tahunterbit_err;?></span>
                        </div>
                        <div class="form-group <?php echo (!empty($jlh_halaman_err)) ? 'has-error' : ''; ?>">
                            <label>Jumlah Halaman</label>
                            <input type="number" name="jlh_halaman" class="form-control" value="<?php echo $jlh_halaman; ?>">
                            <span class="help-block"><?php echo $jlh_halaman_err;?></span>
                        </div>
                        <div class="form-group <?php echo (!empty($penerbit_err)) ? 'has-error' : ''; ?>">
                            <label>Penerbit</label>
                            <input type="text" name="penerbit" class="form-control" value="<?php echo $penerbit; ?>">
                            <span class="help-block"><?php echo $penerbit_err;?></span>
                        </div>
                        <input type="submit" class="btn btn-primary" value="Submit">
                        <a href="index.php" class="btn btn-default">Cancel</a>
                    </form>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
