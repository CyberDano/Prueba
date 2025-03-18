using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameJson : MonoBehaviour
{
    public SaveGame S2;
    // Start is called before the first frame update
    void Start()
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("SaveA"),S2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            SaveGame S = new SaveGame();
            S.playerName = "Cosas";
            S.lives = 69;
            S.health = 420;
            string g = JsonUtility.ToJson(S);
            Debug.Log("Json: " + g);
            PlayerPrefs.SetString("SaveA", g);
        }
    }
}
[System.Serializable]
public class SaveGame
{
    public string playerName;
    public int lives;
    public float health;

}
