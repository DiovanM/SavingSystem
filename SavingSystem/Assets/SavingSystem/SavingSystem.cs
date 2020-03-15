using UnityEngine;
using System.IO;
using System;

public class SavingSystem
{
    private static readonly string rootSavePath = Application.persistentDataPath + "/Saves/";
    private static string currentSlotPath => rootSavePath + SaveSlotSystem.CurrentSlotDirectory;
    private static readonly string fileFormat = ".json";

    public static Action<bool> OnSave;
    public static Action<bool> OnLoad;

    public static bool Save<T>(T _object, string key, string subDirectory = "")
    {
        var path = SaveSlotSystem.CurrentSlotDirectory + subDirectory;
        return SaveToRoot<T>(_object, key, path);
    }

    public static bool SaveToRoot<T>(T _object, string key, string subDirectory = "")
    {
        var path = rootSavePath + subDirectory;
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
        var path = SaveSlotSystem.CurrentSlotDirectory + subDirectory;
        return LoadFromRoot<T>(key, path);
    }

    public static T LoadFromRoot<T>(string key, string subDirectory = "")
    {
        var path = rootSavePath + subDirectory;
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
        var filePath = SaveSlotSystem.CurrentSlotDirectory + subDirectory;
        return SaveExistsOnRoot(filePath);
    }

    public static bool SaveExistsOnRoot(string key, string subDirectory = "")
    {
        var filePath = rootSavePath + subDirectory + key + fileFormat;
        return File.Exists(filePath);
    }

    public static bool DeleteSaveFile(string key, string subDirectory = "")
    {
        var filePath = SaveSlotSystem.CurrentSlotDirectory + subDirectory;
        return DeleteSaveFileOnRoot(key, filePath);
    }

    public static bool DeleteSaveFileOnRoot(string key, string subDirectory = "")
    {
        var filePath = rootSavePath + subDirectory + key + fileFormat;
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveFileSystem] Failed to delete file on address: {filePath} Exception: {e.ToString()}");
                return false;
            }
            Debug.Log($"[SaveFileSystem] File deleted on address: {filePath}");
            return true;
        }
        Debug.LogWarning($"[SaveFileSystem] Failed to delete file on address: {filePath} File does not exist");
        return false;
    }

    public static void DeleteAllSaveFiles(string subDirectory = "")
    {
        var directory = new DirectoryInfo(rootSavePath + subDirectory);
        directory.Delete(true);
        Debug.Log($"[SaveFileSystem] Files deleted on address: {rootSavePath + subDirectory}");
        Directory.CreateDirectory(rootSavePath);
    }

    public static DirectoryInfo[] GetFoldersInDirectory(string startingWith = null, string subDirectory = "")
    {
        var directory = new DirectoryInfo(rootSavePath + subDirectory);
        if (string.IsNullOrEmpty(startingWith))
            return directory.GetDirectories();
        else
            return directory.GetDirectories(startingWith);
    }

}