using System;
using System.IO;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public int playerScore;
    public int playerDeaths;
}

public class DataManager : Singleton<DataManager>
{
    //save Data in JSON format

    private string SavePath => Path.Combine(Application.dataPath, "Data.json"); //persistentDataPath better for a real game

    public void SaveData(PlayerData dataToSave)
    {
        if (dataToSave == null) return;

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("<color=green>Game Saved </color>" + " --> Check Json to visualice saved data " + SavePath);
    }

    public PlayerData LoadData()
    {
        if (File.Exists(SavePath))
        {
            string jsonRead = File.ReadAllText(SavePath);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonRead);

            //foreach (var field in typeof(PlayerData).GetFields())
            //{
            //    Debug.Log(field.Name + ": " + field.GetValue(loadedData));
            //}
            Debug.Log("<color=blue>Data Loaded </color>" + loadedData);

            return loadedData;
        }
        return new PlayerData();
    }
}
