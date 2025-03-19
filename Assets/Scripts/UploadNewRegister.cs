using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UploadNewRegister : MonoBehaviour
{
    [SerializeField] private SessionManager UserData;
    [SerializeField] private string URL; // URL del script PHP
    [SerializeField] private string regis_name; // Nuevo registro

    /// <summary>
    /// Inicia la petición para crear un nuevo registro.
    /// </summary>
    public void NewRegister()
    {
        StartCoroutine(Request());
    }

    public void SetRegisterName(string name)
    {
        regis_name = name;
    }
    private IEnumerator Request()
    {
        // Crear el formulario con los datos del usuario
        WWWForm form = new WWWForm();        
        form.AddField("mail", UserData.sessionManager.session.mail); // Correo para el usuario
        form.AddField("pass", UserData.sessionManager.session.pass); // Clave para el usuario
        form.AddField("new_register", regis_name); // Nuevo registro

        // Realizar la solicitud POST al servidor
        UnityWebRequest ask = UnityWebRequest.Post(URL, form);

        // Enviar la solicitud y esperar la respuesta
        yield return ask.SendWebRequest();

        if (ask.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Message: {ask.downloadHandler.text}"); // Respuesta
        }
        else Debug.Log($"Error: {ask.error}"); // Mostrar error en caso de fallo         
    }
}