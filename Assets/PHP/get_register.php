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
    $register = $_POST['register'];
    // Consulta preparada para obtener el hash de la contraseña y registros
    $stmt = $con->prepare("SELECT ID_user, registers, pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Vincular parámetro
    $stmt->execute();
    $result = $stmt->get_result();
    if ($row = $result->fetch_assoc()) {
        $hashedPassword = $row['pass']; // Obtener el hash de la contraseña
        $id_user = $row['ID_user']; // ID del usuario
        $json_data = $row['registers']; // JSON del campo registers
        // Verificar la contraseña
        if (password_verify($pass, $hashedPassword)) {
            // Decodificar el JSON y procesar registros
            $array_data = json_decode($json_data, true);
            if (is_array($array_data) && !empty($array_data)) {
                // Obtener el registro correspondiente
                $register_name = $array_data[$register - 1];
                echo $register_name;
            } else {echo "El campo JSON está vacío o no es válido.";}
            // Consulta preparada para obtener registros adicionales
            $stmt2 = $con->prepare("
                SELECT r.world, r.level FROM register_list r
                INNER JOIN users u ON r.id_user = u.ID_user
                WHERE r.id_user = ? AND r.register = ?
                ORDER BY r.world DESC, r.level DESC LIMIT 1 ");
            $stmt2->bind_param("ii", $id_user, $register); // Vincular parámetros
            $stmt2->execute();
            $result2 = $stmt2->get_result();
            if ($row2 = $result2->fetch_assoc()) {echo "=JSON=" . json_encode($row2); /* Devolver JSON de los datos */}
            else {echo json_encode(array("error" => "No se encontraron registros adicionales."));}
            $stmt2->close(); // Cerrar declaración preparada
        } else {echo "Contraseña incorrecta.";}
    } else {echo "El usuario no existe.";}
    $stmt->close(); // Cerrar declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>