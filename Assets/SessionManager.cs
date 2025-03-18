using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private string URL;
    [SerializeField] private string nick;
    [SerializeField] private string user;
    [SerializeField] private string password;
    [SerializeField] private bool keepLog;
    public Session sessionManager;
    public Toggle toggle;
    public TextMeshProUGUI textJSON;

    private void Start()
    {
        toggle.isOn = PlayerPrefs.GetInt("keepLog") == 1;
        if (toggle.isOn) AutoLogin();
    }
    private void Update()
    {
        try
        {
            if (sessionManager.session.nick != "") textJSON.text = $"You are: <color=blue>{sessionManager.session.nick}</color>\nYour mail: <color=blue>{sessionManager.session.mail}</color>";
            else textJSON.text = $"You are: <color=blue>\"\"</color>\nYour mail: <color=blue>{sessionManager.session.mail}</color>";
        }
        catch { textJSON.text = ""; }
    }
    public void SetNick(string nickname)
    {
        nick = nickname;
        sessionManager.session.nick = nickname;
    }
    public void SetMail(string email) { user = email; }
    public void SetPass(string pass) { password = pass; }
    /// <summary>
    /// Alterna entre mantener o no mantener la sesión iniciada
    /// </summary>
    public void KeepLogToggle()
    {
        keepLog = !keepLog;
        // Guarda JSON de credenciales
        if (keepLog) PlayerPrefs.SetInt("keepLog", 1);
        // No guarda JSON de credenciales
        else PlayerPrefs.SetInt("keepLog", 0);
        toggle.isOn = PlayerPrefs.GetInt("keepLog") == 1;
    }
    /// <summary>
    /// Accede al PHP con las credenciales de usuario
    /// </summary>
    public void Login()
    {
        sessionManager = new Session(nick, user, password);
        if (PlayerPrefs.GetInt("keepLog") == 1) SaveSessionToken();
        StartCoroutine(nameof(UserLogin));
    }
    /// <summary>
    /// Accede al PHP con las credenciales de usuario
    /// Recurre a las credenciales guardadas si "keep log" es verdadero (valor 1)
    /// </summary>
    public void AutoLogin()
    {
        string json = GetSessionToken(); // Recurre al json guardado
        if (PlayerPrefs.GetInt("keepLog") == 1 && json != "")
        {
            // Deserializa los datos
            UserSession userSessionData = JsonUtility.FromJson<UserSession>(json);
            if (sessionManager == null) sessionManager = new Session("", "", ""); // Inicializa si no existe
            // Actualiza los valores de la sesión actual
            sessionManager.session.nick = userSessionData.nick;
            sessionManager.session.mail = userSessionData.mail;
            sessionManager.session.pass = userSessionData.pass;
            StartCoroutine(nameof(UserLogin));
        }
    }
    /// <summary>
    /// Modifica el apodo en la entrada del usuario en base de datos
    /// </summary>
    public void SetNickname()
    {
        if (sessionManager != null) StartCoroutine(nameof(UserNickManage));
        else Debug.Log("There's no user logged");
    }
    /// <summary>
    /// Alterna cuando el usuario se conecta a PhotonNetwork
    /// </summary>
    /// <param name="action"></param>
    public void SessionToggle(string action)
    {
        if (sessionManager != null) StartCoroutine("UserSessionToggle", action);
        else Debug.Log("There's no user logged");
    }
    /// <summary>
    /// Cierra sesión
    /// </summary>
    public void Logout()
    {        
        sessionManager.ClearSession();
        ClearSessionToken();
        Debug.Log("Loged out.");
    }
    /* ------------------------- JSON PARA GUARDAR SESIÓN ------------------------- */
    /// <summary>
    /// Crea el JSON de la sesión activa en PlayerPrefs si "keepLog" es verdadero
    /// </summary>
    void SaveSessionToken()
    {
        string json = JsonUtility.ToJson(sessionManager.session);
        string encryptedJson = FileSecurity.Encrypt(json);
        PlayerPrefs.SetString("session", encryptedJson);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Devuelve el JSON de la sesión activa
    /// </summary>
    string GetSessionToken()
    {
        string encryptedJson = PlayerPrefs.GetString("session", null);
        if (!string.IsNullOrEmpty(encryptedJson)) return FileSecurity.Decrypt(encryptedJson);
        else return null;
    }
    /// <summary>
    /// Limpia la sesión de PlayerPrefs
    /// </summary>
    void ClearSessionToken()
    {
        PlayerPrefs.DeleteKey("session");
        PlayerPrefs.Save();
    }
    /* ---------------------------------------------------------------------------- */
    /// <summary>
    /// Inicia sesión en PHP
    /// </summary>
    /// <returns></returns>
    IEnumerator UserLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("mail", sessionManager.session.mail);
        form.AddField("pass", sessionManager.session.pass);
        UnityWebRequest ask = UnityWebRequest.Post(URL + "login.php", form);
        {
            yield return ask.SendWebRequest();

            if (ask.result == UnityWebRequest.Result.Success)
            {
                string[] text = ask.downloadHandler.text.Split("=NICK=");
                Debug.Log(text[0]);
                sessionManager.session.nick = text[1];
                if (keepLog)
                {
                    SaveSessionToken();
                }
            }
            else
            {
                Debug.LogError("Error en el inicio de sesión: " + ask.error);
            }
        }
    }
    /// <summary>
    /// Modifica el nick del usuario en PHP
    /// </summary>
    /// <returns></returns>
    IEnumerator UserNickManage()
    {
        WWWForm form = new WWWForm();
        form.AddField("mail", sessionManager.session.mail);
        form.AddField("pass", sessionManager.session.pass);
        form.AddField("nick", sessionManager.session.nick);
        UnityWebRequest ask = UnityWebRequest.Post(URL + "setnick.php", form);
        {
            yield return ask.SendWebRequest();

            if (ask.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(ask.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + ask.error);
            }
        }
    }
    /// <summary>
    /// Cambia el estado de "offline" a "online" o viceversa en PHP
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IEnumerator UserSessionToggle(string action)
    {
        WWWForm form = new WWWForm();
        form.AddField("mail", sessionManager.session.mail);
        form.AddField("pass", sessionManager.session.pass);
        form.AddField("action", action); // "online" u "offline"
        UnityWebRequest ask = UnityWebRequest.Post(URL + "usersession.php", form);
        {
            yield return ask.SendWebRequest();

            if (ask.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(ask.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + ask.error);
            }
        }
    }
}