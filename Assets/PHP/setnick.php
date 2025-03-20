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
    $nick = $_POST['nick'];
    // Consulta preparada para obtener el hash de la contraseña y el ID del usuario
    $stmt = $con->prepare("SELECT ID_user, pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Asociar parámetros
    $stmt->execute(); // Ejecutar consulta
    $result = $stmt->get_result(); // Obtener resultado
    if ($row = $result->fetch_assoc()) {
        $hashedPassword = $row['pass']; // Obtener el hash de la contraseña
        $id_user = $row['ID_user']; // Obtener el ID del usuario
        // Verificar la contraseña ingresada contra el hash almacenado
        if (password_verify($pass, $hashedPassword)) {
            // Consulta preparada para actualizar el nick del usuario
            $updateStmt = $con->prepare("UPDATE users SET name = ? WHERE ID_user = ?");
            $updateStmt->bind_param("si", $nick, $id_user); // Asociar parámetros (nombre y ID)
            if ($updateStmt->execute()) {echo "Your nick is now '$nick'";}
            else {echo "Error updating nick: " . $con->error;}
            $updateStmt->close(); // Cerrar declaración preparada
        } else {echo "Contraseña incorrecta.";}
    } else {echo "User doesn't exist.";}    
    $stmt->close(); // Cerrar declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>