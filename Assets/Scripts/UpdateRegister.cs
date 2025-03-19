using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpdateRegister : MonoBehaviour
{
    [Header("Referenciar a SessionManager.cs")]
    [SerializeField] private SessionManager UserData;
    [SerializeField] private string URL; // URL del script PHP
    [Header("�ndice del registro")][SerializeField] private int register;
    [Header("�ndice del mundo")][SerializeField] private int world;
    [Header("�ndice del nivel")][SerializeField] private int level;

    /// <summary>
    /// Inicia la petici�n para actualizar los datos del nivel.
    /// </summary>
    public void UpdateLevelData()
    {
        StartCoroutine(Request());
    }
    private IEnumerator Request()
    {
        // Crear el formulario con los datos del usuario
        WWWForm form = new WWWForm();
        form.AddField("mail", UserData.sessionManager.session.mail); // Correo para el usuario
        form.AddField("pass", UserData.sessionManager.session.pass); // Clave para el usuario
        form.AddField("register", register); // �ndice del registro (1, 2, 3)
        form.AddField("world", world); // �ndice del mundo (1, 2, 3)
        form.AddField("level", level); // �ndice del nivel ( 1, 2, 3, 4, 5)

        // Realizar la solicitud POST al servidor
        UnityWebRequest ask = UnityWebRequest.Post(URL + "updateregister.php", form);

        // Enviar la solicitud y esperar la respuesta
        yield return ask.SendWebRequest();

        if (ask.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Message: {ask.downloadHandler.text}"); // Respuesta
        }
        else Debug.Log($"Error: {ask.error}"); // Mostrar error en caso de fallo         
    }
}