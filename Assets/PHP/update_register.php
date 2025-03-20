<?php
/* Abre acceso */
$con = mysqli_connect("fdb1028.awardspace.net", "4594937_stickrock", "Z,AAZykL9]YZDi)}", "4594937_stickrock");
if (!$con) {
    echo 'No se pudo conectar con la base de datos...';
    echo "Failed to connect to MySQL: " . mysqli_connect_error();
} else {
    /* Credenciales obtenidas de Unity */
    $mail = $_POST['mail'];
    $pass = $_POST['pass'];
    $register = $_POST['register'];
    $world = $_POST['world'];
    $level = $_POST['level'];
    // Consulta para obtener el hash de la contraseña del usuario
    $sql = "SELECT ID_user, pass FROM users WHERE mail = ?";
    $stmt = $con->prepare($sql);
    $stmt->bind_param("s", $mail);
    $stmt->execute();
    $result = $stmt->get_result();
    if ($row = $result->fetch_assoc()) {
        $hashedPassword = $row['pass'];
        $id_user = $row['ID_user'];
        // Verifica la contraseña ingresada contra el hash
        if (password_verify($pass, $hashedPassword)) {
            // Actualizar el campo 'passed' solo si el valor es menor que 3
            $updateSql = "UPDATE register_list 
                          SET passed = passed + 1 
                          WHERE id_user = ? 
                            AND register = ? 
                            AND world = ? 
                            AND level = ?
                            AND passed < 3";
            $updateStmt = $con->prepare($updateSql);
            $updateStmt->bind_param("iiii", $id_user, $register, $world, $level);
            if ($updateStmt->execute()) {
                // Verifica si se actualizó alguna fila
                if ($updateStmt->affected_rows > 0) {echo "Las estrellas obtenidas del nivel aumentaron.";}
                else {echo "El registro del nivel no existe o no puede aumentar.";}
            } else {echo "Error al actualizar el nivel:" . $con->error;}
            $updateStmt->close();
        } else {echo "Contraseña incorrecta.";}
    } else {echo "El usuario no existe.";}
    $stmt->close();
}
/* Cierra acceso */
mysqli_close($con);
?>