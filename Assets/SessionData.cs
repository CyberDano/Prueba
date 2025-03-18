using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class UserSession
{
    public string nick; // Apodo del usuario
    public string mail; // Correo del usuario
    public string pass; // Clave del usuario
}
public class Session
{
    public UserSession session;

    // Constructor para la sesi�n
    public Session(string nickname, string email, string passd)
    {
        session = new UserSession
        {
            nick = nickname,
            mail = email,
            pass = passd
        };
    }
    /// <summary>
    /// M�todo para limpiar los datos al cerrar sesi�n
    /// </summary>
    public void ClearSession() { session = null; }
}
/* ------------------------- CLASE PARA CIFRAR CREDENCIALES ------------------------- */
public class FileSecurity
{
    private static readonly string key = "StickAndRock2025"; // Clave AES-16 (16, 24 o 32 caracteres)
    /// <summary>
    /// Encripta los datos
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV(); // IV aleatorio para cada cifrado

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length); // Guarda el IV en el archivo
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] data = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(data, 0, data.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
    /// <summary>
    /// Desencripta los datos
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string Decrypt(string cipherText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipherBytes = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}