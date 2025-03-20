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
    $registerIndex = $_POST['register'];
    // Consulta preparada para obtener el hash de la contraseña y registros
    $stmt = $con->prepare("SELECT id_user, registers, pass FROM users WHERE mail = ?");
    $stmt->bind_param("s", $mail); // Vincular parámetro
    $stmt->execute();
    $result = $stmt->get_result();
    if ($row = $result->fetch_assoc())
    {
        $id_user = $row['id_user']; // Obtén el id del usuario
        $hashedPassword = $row['pass']; // Obtener el hash de la contraseña
        $registersJson = $row['registers']; // JSON actual del campo registers
        // Verificar la contraseña ingresada contra el hash almacenado
        if (password_verify($pass, $hashedPassword))
        {
        	/* AQUI HACER LA GESTION DE BORRAR EL REGISTRO */                
            // Paso 1: Obtener los registros actuales del usuario
            $stmt = $con->prepare("SELECT registers FROM users WHERE id_user = ?");
            $stmt->bind_param("i", $id_user);
            $stmt->execute();
            $result = $stmt->get_result();
            if ($row = $result->fetch_assoc())
            {
                $registersArray = json_decode($row['registers'], true);
                if (isset($registersArray[$registerIndex-1]))
                {
                    // Paso 2: Eliminar el registro del array JSON
                    $oldRegister = $registersArray[$registerIndex-1]; // Guardar el antiguo nombre
                    $registersArray[$registerIndex-1] = "";
                    $updatedJson = json_encode(array_values($registersArray)); // Re-indexar el array
                    // Paso 3: Actualizar el campo 'registers' en la tabla 'users'
                    $updateStmt = $con->prepare("UPDATE users SET registers = ? WHERE id_user = ?");
                    $updateStmt->bind_param("si", $updatedJson, $id_user);
                    if ($updateStmt->execute())
                    {
                        echo "Registro '$oldRegister' ha sido eliminado.";
                        // Paso 4: Eliminar las entradas correspondientes en 'register_list'
                        $deleteStmt = $con->prepare("DELETE FROM register_list WHERE id_user = ? AND register = ?");
                        $deleteStmt->bind_param("ii", $id_user, $registerIndex);
                        if ($deleteStmt->execute()) { }
                        else {echo "Error al eliminar de 'register_list': " . $deleteStmt->error;}
                    } else {echo "Error al actualizar el array JSON: " . $updateStmt->error;}
                } // isset()
             } else {echo "El índice del registro no existe en el array JSON.";}
        } else {echo "Contraseña incorrecta.";}
    } else {echo "El usuario no existe.";}
    $stmt->close(); // Cerrar la declaración preparada
}
/* Cierra acceso */
mysqli_close($con);
?>