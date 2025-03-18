using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PHPRequest : MonoBehaviour
{
    public string URL = "https://stickrock.freewebhostmost.com/test.php";
    /// <summary>
    /// Empieza la corrutina
    /// </summary>
    public void GoRequest()
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
        ask = UnityWebRequest.Post(URL, form); // Método POST
        yield return ask.SendWebRequest(); // Realiza el envío de la consulta
        while (!ask.isDone) // Mientras no se reciba nada...
        {
            Debug.LogWarning("Asking..."); // En espera
            yield return null;
        }
        Debug.LogWarning("Message: " + ask.downloadHandler.text); // Resultado exitoso
    }
}