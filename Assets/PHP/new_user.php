<?php
/* Abre acceso */
$con = mysqli_connect("fdb1028.awardspace.net", "4594937_stickrock", "Z,AAZykL9]YZDi)}", "4594937_stickrock");
if (!$con) {
    echo 'No se pudo conectar con la base de datos...';
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
} else {
    /* Credenciales */
    $nick = $_POST['nick'];
    $mail = $_POST['mail'];
    $pass = $_POST['pass'];
    // Consulta preparada para verificar si el correo ya está en uso
    $stmt = $con->prepare("SELECT * FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Asociar parámetro
    $stmt->execute(); // Ejecutar consulta
    $result = $stmt->get_result();
    if ($result->num_rows > 0) {
        echo "El correo ya está en uso.";
    } else {
        // Hashear la contraseña
        $hashedPassword = password_hash($pass, PASSWORD_DEFAULT);
        // Consulta preparada para registrar un nuevo usuario
        if (empty($nick)) {
            $stmt = $con->prepare("INSERT INTO users (mail, pass) VALUES (?, ?)");
            $stmt->bind_param("ss", $mail, $hashedPassword); // Asociar parámetros
        } else {
            $stmt = $con->prepare("INSERT INTO users (name, mail, pass) VALUES (?, ?, ?)");
            $stmt->bind_param("sss", $nick, $mail, $hashedPassword); // Asociar parámetros
        }
        if ($stmt->execute()) {echo "El usuario se ha dado de alta.";}
        else {echo "Error al dar de alta al usuario: " . $con->error;}
    }
    $stmt->close(); // Cerrar la declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>