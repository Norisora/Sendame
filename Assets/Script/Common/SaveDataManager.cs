using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveDataManager
{
    public static SaveDataManager Instance = new SaveDataManager();
    private SaveDataManager() { }

    public static readonly string DeckSaveKey = "DeckSaveKey";    //セーブデータキー
    
    public enum SaveType
    {
        Deck,

    }

    public void Save<T>(T saveObject, SaveType saveType, int param = 0)
    {
        
        var saveKey = GetSaveKey(saveType, param);
        PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(saveObject));
    }

    public T Load<T>(SaveType saveType, int param = 0)
    {
        var saveKey = GetSaveKey(saveType, param);
        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(saveKey));
    }

    public bool HasSaveData(SaveType saveType, int param = 0)
    {
        var saveKey = GetSaveKey(saveType, param);
        return PlayerPrefs.HasKey(saveKey);
    }
    string GetSaveKey(SaveType saveType, int param)
    {
        switch (saveType)
        {
            case SaveType.Deck:
                return DeckSaveKey + param.ToString();
        }
        return null;
    }

}
