using UnityEngine;
using System.IO;
using System;

public class SaveFileSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/Saves/";
    private static readonly string fileFormat = ".json";

    public static Action<bool> OnSave;
    public static Action<bool> OnLoad;

    public static bool Save<T>(T _object, string key, string subDirectory = "")
    {
        var path = savePath + subDirectory;
        Directory.CreateDirectory(path);
        var jsonObject = JsonUtility.ToJson(_object);

        try
        {
            File.WriteAllText(path + key + fileFormat, jsonObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveFileSystem] Error trying to save file: {path + key + fileFormat} Exception: {e.ToString()}");
            OnSave?.Invoke(false);
            return false;
        }

        Debug.Log($"[SaveFileSystem] File saved on address: {path + key + fileFormat}");
        OnSave?.Invoke(true);
        return true;
    }

    public static T Load<T>(string key, string subDirectory = "")
    {
        var path = savePath + subDirectory;
        T returnData = default(T);
        var data = default(string);

        try 
        {
            data = File.ReadAllText(path + key + fileFormat);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveFileSystem] Error trying to load file: {path + key + fileFormat} Exception: {e.ToString()}");
            OnLoad?.Invoke(false);
            return returnData;
        }

        Debug.Log($"[SaveFileSystem] File loaded from address: {path + key + fileFormat}");
        returnData = JsonUtility.FromJson<T>(data);

        OnLoad?.Invoke(true);
        return returnData;
    }

    public static bool SaveExists(string key, string subDirectory = "")
    {
        var filePath = savePath + subDirectory + key + fileFormat;
        return File.Exists(filePath);
    }

    public static void DeleteSaveFiles(string subDirectory = "")
    {
        var directory = new DirectoryInfo(savePath + subDirectory);
        directory.Delete(true);
        Debug.Log($"[SaveFileSystem] Files deleted on address: {savePath + subDirectory}");
        Directory.CreateDirectory(savePath);
    }

}