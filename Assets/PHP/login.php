<?php
/* Abre acceso */
$con = mysqli_connect("fdb1028.awardspace.net", "4594937_stickrock", "Z,AAZykL9]YZDi)}", "4594937_stickrock");
if (!$con) {
    echo 'No se pudo conectar con la base de datos...';
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
} else {   
    /* Credenciales */
    $mail = $_POST['mail'];
    $pass = $_POST['pass'];
    // Consulta preparada para obtener el hash de la contraseña
    $stmt = $con->prepare("SELECT pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Asociar parámetros
    $stmt->execute(); // Ejecutar consulta
    $result = $stmt->get_result(); // Obtener resultados
    if ($row = $result->fetch_assoc()) {
        $hashedPassword = $row['pass']; // Obtener el hash de la contraseña
        // Verifica la contraseña ingresada contra el hash
        if (password_verify($pass, $hashedPassword)) {
            // Contraseña correcta, iniciar sesión
            echo "Log in as $mail";
            // Consulta preparada para obtener el nombre del usuario
            $stmt = $con->prepare("SELECT name FROM users WHERE mail = ?");
            $stmt->bind_param("s", $mail);
            $stmt->execute();
            $result = $stmt->get_result();
            if ($row = $result->fetch_assoc()) {echo "=NICK=" . $row['name'];}
        } else {echo "Contraseña incorrecta.";}
    } else {echo "User doesn't exist.";
    }
    $stmt->close(); // Cerrar la declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>