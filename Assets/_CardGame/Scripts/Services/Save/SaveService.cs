using System.IO;
using UnityEngine;

public class SaveService : ISaveService
{
    private string savePath => Path.Combine(Application.persistentDataPath, "matchgame_save.json");

    public void SaveGame(GameSaveData data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
#if UNITY_EDITOR
        Debug.Log($"Game saved to {savePath}");
#endif
    }

    public GameSaveData LoadGame()
    {
        if (!File.Exists(savePath))
            return null;

        try
        {
            var json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Load failed: {ex.Message}");
            return null;
        }
    }

    public bool HasSave()
    {
        if (!File.Exists(savePath))
            return false;
        else
            return true;
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}