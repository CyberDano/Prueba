using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Register_tile
{    
    public int world; // Número del mundo (1, 3)
    public int level; // Número del nivel (1, 5)
    public Register_tile(int world, int level)
    {
        this.world = world;
        this.level = level;
    }
}
public class RegisterRequest : MonoBehaviour
{
    public string URL;
    [SerializeField] private int register;
    [SerializeField] private string register_name;
    [SerializeField] private SessionManager UserData;
    /// <summary>
    /// Comienza la corutina de obtención de los datos del registro del usuario
    /// </summary>
    public void RequestRegister()
    {
        StartCoroutine(Request());
    }
    /// <summary>
    /// Consulta en línea al script de una URL
    /// </summary>
    /// <returns>Obtienes el resultado de la llamada si es exitoso.</returns>
    public IEnumerator Request()
    {
        yield return null;
        UnityWebRequest ask = new UnityWebRequest(); // Consulta web
        WWWForm form = new WWWForm(); // Clase necesaria para la consulta
        form.AddField("register", register); // Pasa el índice numérico del registro (1, 2, 3)
        form.AddField("mail", UserData.sessionManager.session.mail);
        form.AddField("pass", UserData.sessionManager.session.pass);
        ask = UnityWebRequest.Post(URL + "getregister.php", form); // Método POST
        yield return ask.SendWebRequest(); // Realiza el envío de la consulta
        if (ask.result == UnityWebRequest.Result.Success)
        {
            string[] text = ask.downloadHandler.text.Split("=JSON=");            
            register_name = text[0];
            Debug.Log("Register name "+text[0]);
            Debug.Log(text[1]);
            Register_tile tile = JsonUtility.FromJson<Register_tile>(text[1]);
            Debug.Log($"Max. world reached: {tile.world} - Max. level reached: {tile.level}");
        }
        else
        {
            Debug.LogError("Error: " + ask.error);
        }
    }
}