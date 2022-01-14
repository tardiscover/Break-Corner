using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveData
{
    const string saveFileName = "saveData.json";

    public static void SaveGameData()
    {
        string path = Application.persistentDataPath + "/" + saveFileName;

        if (!File.Exists(path))
        {
            File.Create(path);
        }

        string json = JsonUtility.ToJson(GameManager.gameData);

        StreamWriter writer = new StreamWriter(path);
        writer.Write(json);
        writer.Close();
    }

    public static void LoadGameData()
    {
        string path = Application.persistentDataPath + "/" + saveFileName;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (json != "")
            {
                GameManager.gameData = JsonUtility.FromJson<GameData>(json);
            }
        }
    }
}
