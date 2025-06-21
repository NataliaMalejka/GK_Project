using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int startLevelIndex;
    public int playerGold;
    public Item playerWeapon;
    public int[] costItemList;
}

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "AstroGame.json");

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveSystem] Game saved to {SavePath}");
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveSystem] Save file not found.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"[SaveSystem] Game loaded from {SavePath}");
        return data;
    }
}
