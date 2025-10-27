using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(PlayerData data, bool prettyPrint = true)
    {
        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint);

            File.WriteAllText(SavePath, json);

            Debug.Log("저장완료 : " + SavePath);
            Debug.Log(json);

            Managers.Game.SaveSuccess = true;
        }
        catch(Exception e)
        {
            Debug.Log(e);
            Managers.Game.SaveSuccess = false;
        }
    }

    public static bool TryLoad(out PlayerData data)
    {
        if (!File.Exists(SavePath))
        {
            data = PlayerData.GetDefault();
            Debug.Log("저장 파일이 없으니 디폴트 로드");
            return false;
        }

        string json = File.ReadAllText(SavePath);

        data = JsonUtility.FromJson<PlayerData>(json);

        Debug.Log("불러오기 성공 : " + SavePath);
        Debug.Log(json);
        return true;
    }
}