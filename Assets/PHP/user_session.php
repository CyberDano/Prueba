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
    $action = $_POST['action'];
    // Consulta preparada para obtener el hash de la contraseña
    $stmt = $con->prepare("SELECT pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Asociar parámetro
    $stmt->execute(); // Ejecutar la consulta
    $result = $stmt->get_result(); // Obtener los resultados
    if ($row = $result->fetch_assoc()) {
        $hashedPassword = $row['pass']; // Hash de la contraseña
        // Verificar la contraseña ingresada contra el hash almacenado
        if (password_verify($pass, $hashedPassword)) {
            // Consulta preparada para actualizar el campo 'session'
            $updateStmt = $con->prepare("UPDATE users SET session = ? WHERE mail = ?");
            $updateStmt->bind_param("ss", $action, $mail); // Asociar parámetros (acción y correo)
            if ($updateStmt->execute()) {echo "You are now $action";}
            else {echo "Error updating session: " . $con->error;}
            $updateStmt->close(); // Cerrar la declaración preparada
        } else {echo "Contraseña incorrecta.";}
    } else {echo "User doesn't exist.";}
    $stmt->close(); // Cerrar la declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>