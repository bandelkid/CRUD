<?php
require_once "config.php";

// Inisialisasi variabel
$username = $password = "";
$username_err = $password_err = "";

// Proses data formulir saat tombol login ditekan
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Validasi username
    if (empty(trim($_POST["username"]))) {
        $username_err = "Please enter your username.";
    } else {
        $username = trim($_POST["username"]);
    }

    // Validasi password
    if (empty(trim($_POST["password"]))) {
        $password_err = "Please enter your password.";
    } else {
        $password = trim($_POST["password"]);
    }

    // Jika tidak ada kesalahan validasi
    if (empty($username_err) && empty($password_err)) {
        // Query untuk mencari pengguna dengan username yang sesuai
        $query = "SELECT id, username, password FROM users WHERE username = ?";
        $stmt = mysqli_prepare($link, $query);

        if ($stmt) {
            // Bind variabel ke pernyataan persiapan sebagai parameter
            mysqli_stmt_bind_param($stmt, "s", $param_username);

            // Set parameter
            $param_username = $username;

            // Coba eksekusi pernyataan persiapan
            if (mysqli_stmt_execute($stmt)) {
                // Simpan hasil
                mysqli_stmt_store_result($stmt);

                if (mysqli_stmt_num_rows($stmt) == 1) {
                    // Bind hasil ke variabel
                    mysqli_stmt_bind_result($stmt, $id, $username, $hashed_password);

                    if (mysqli_stmt_fetch($stmt)) {
                        // Verifikasi password
                        if (password_verify($password, $hashed_password)) {
                            // Password cocok, proses login
                            echo "Login berhasil!";
                        } else {
                            // Password tidak cocok, tampilkan pesan error
                            $password_err = "Invalid password. Please try again.";
                        }
                    }
                } else {
                    // Pengguna tidak ditemukan, tampilkan pesan error
                    $username_err = "No account found with that username.";
                }
            } else {
                echo "Oops! Something went wrong. Please try again later.";
            }
        }

        // Tutup pernyataan
        mysqli_stmt_close($stmt);
    }

    // Tutup koneksi
    mysqli_close($link);
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Login</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.css">
    <style type="text/css">
        body{ font: 14px sans-serif; }
        .wrapper{ width: 350px; padding: 20px; }
    </style>
</head>
<body>
    <div class="wrapper">
        <h2>Login</h2>
        <p>Please fill in your credentials to login.</p>
        <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
            <div class="form-group <?php echo (!empty($username_err)) ? 'has-error' : ''; ?>">
                <label>Username</label>
                <input type="text" name="username" class="form-control" value="<?php echo $username; ?>">
                <span class="help-block"><?php echo $username_err; ?></span>
            </div>    
            <div class="form-group <?php echo (!empty($password_err)) ? 'has-error' : ''; ?>">
                <label>Password</label>
                <input type="password" name="password" class="form-control">
                <span class="help-block"><?php echo $password_err; ?></span>
            </div>
            <div class="form-group">
                <input type="submit" class="btn btn-primary" value="Login">
            </div>
            <p>Don't have an account? <a href="register.php">Sign up now</a>.</p>
        </form>
    </div>    
</body>
</html>
