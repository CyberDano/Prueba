using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UploadUser : MonoBehaviour
{
    [Header("URL Y DATOS DE SESIÓN")]
    [SerializeField] private string URL; // URL del script PHP
    [SerializeField] private string nickname; // Apodo del usuario
    [SerializeField] private string mail; // Email del usuario
    [SerializeField] private string pass; // Contraseña del usuario

    /// <summary>
    /// Inicia la petición para registrar un usuario.
    /// </summary>
    public void UploadNewUser()
    {
        StartCoroutine(Request());
    }

    public void SetNick(string nick) {
        nickname = nick;
    }
    public void SetMail(string email)
    {
        mail = email;
    }
    public void SetPass(string password)
    {
        pass = password;
    }
    private IEnumerator Request()
    {
        // Crear el formulario con los datos del usuario
        WWWForm form = new WWWForm();
        form.AddField("nick", nickname); // Nick para el usuario
        form.AddField("mail", mail); // Correo para el usuario
        form.AddField("pass", pass); // Clave para el usuario

        // Realizar la solicitud POST al servidor
        UnityWebRequest ask = UnityWebRequest.Post(URL, form);

        // Enviar la solicitud y esperar la respuesta
        yield return ask.SendWebRequest();

        if (ask.result == UnityWebRequest.Result.Success) Debug.Log($"Respuesta del servidor: {ask.downloadHandler.text}"); // Mostrar respuesta del servidor
        else Debug.LogError($"Error: {ask.error}"); // Mostrar error en caso de fallo            
    }
}