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
    $newRegister = $_POST['new_register']; // Nuevo archivo de guardado
    // Consulta preparada para obtener el hash de la contraseña y registros
    $stmt = $con->prepare("SELECT id_user, registers, pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Vincular parámetro
    $stmt->execute();
    $result = $stmt->get_result();
    if ($row = $result->fetch_assoc()) {
        $id_user = $row['id_user']; // Obtén el id del usuario
        $hashedPassword = $row['pass']; // Obtener el hash de la contraseña
        $registersJson = $row['registers']; // JSON actual del campo registers
        // Verificar la contraseña ingresada contra el hash almacenado
        if (password_verify($pass, $hashedPassword)) {
            // JSON a array
            $registersArray = json_decode($registersJson, true);
            if (!is_array($registersArray)) {$registersArray = [];}
            $registerIndex = null;
            // Verificar si hay entradas vacías en el array JSON
            for ($i = 0; $i < 3; $i++) {
                if (isset($registersArray[$i]) && $registersArray[$i] == "") {
                    $registerIndex = $i; // Encontrar índice vacío
                    break;
                }
            }
            // Si no hay entradas vacías, pero el array tiene menos de 3 elementos
            if ($registerIndex === null && count($registersArray) < 3) {$registerIndex = count($registersArray);}
            // Si se encontró un índice válido, proceder con la lógica
            if ($registerIndex !== null && trim($newRegister) !== "") {
                $registersArray[$registerIndex] = $newRegister; // Añadir al array en el índice
                $updatedJson = json_encode($registersArray); // Actualizar JSON                
                $updateStmt = $con->prepare("UPDATE users SET registers = ? WHERE mail = ?"); // Actualizar 'registers' con el nuevo JSON
                $updateStmt->bind_param("ss", $updatedJson, $mail);
                if ($updateStmt->execute()) {
                    echo "Nuevo archivo añadido: $newRegister";
                    // Usar $registerIndex + 1 para la tabla 'register_list'
                    $registerListIndex = $registerIndex + 1; // Para valores 1, 2, 3
                    // Añadir a la tabla 'register_list'
                    $AddStmt = $con->prepare("INSERT INTO register_list (id_user, register, world, level, passed) VALUES (?, ?, 1, 1, 0)");
                    $AddStmt->bind_param("ii", $id_user, $registerListIndex);
                    if ($AddStmt->execute()) { }
                    else {echo "Error al añadir entrada en 'register_list': " . $AddStmt->error;}
                    $AddStmt->close();
                } else {echo "Error al actualizar 'registers': " . $updateStmt->error;}
                $updateStmt->close();
            } else {echo "Error: No se puede añadir más registros o el nuevo registro está vacío.";}
        } else {echo "Contraseña incorrecta.";}
    } else {echo "El usuario no existe.";}
    $stmt->close(); // Cerrar la declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>