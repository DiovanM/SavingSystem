using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class SaveSlotSystem : MonoBehaviour
{
    public static int currentSlotId { get; private set; } = 1;
    public static SaveSlotBaseData currentSaveSlotData;

    private static readonly string saveDirectory = "Save";
    private static readonly string saveSlotKey = "SlotData";

    public static string CurrentSlotDirectory => $"{saveDirectory + currentSlotId}/";// == null ? $"{saveDirectory + 1}/" : $"{saveDirectory + currentSaveSlotData.id}/";

    public static bool SetCurrentSlot(int id)
    {
        var saveSlots = GetSaveSlotsData();

        foreach (var slot in saveSlots)
        {
            if (slot.id == id)
            {
                currentSlotId = id;
                currentSaveSlotData = slot;
                return true;
            }
        }
        Debug.LogError("[SaveSlotSystem] Error setting current slot id - No slot found with provided id");
        return false;
    }

    public static void UpdateCurrentSlotData(string name, float playTime, float completionPercentage)
    {
        var data = new SaveSlotBaseData(currentSaveSlotData.id, name, playTime, completionPercentage);
        SavingSystem.SaveToRoot(data, saveSlotKey, $"{saveDirectory + data.id}/");
    }

    public static void SaveSlotData(SaveSlotBaseData data)
    {
        SavingSystem.SaveToRoot(data, saveSlotKey, $"{saveDirectory + data.id}/");
    }

    public static List<SaveSlotBaseData> GetSaveSlotsData()
    {
        var saveFolders = GetAllSaveFolders();
        var saveFiles = new List<SaveSlotBaseData>();
        foreach (var folder in saveFolders)
        {
            var file = SavingSystem.LoadFromRoot<SaveSlotBaseData>(saveSlotKey, $"{folder.Name}/");
            if (file != null)
                saveFiles.Add(file);
        }

        return saveFiles;
    }

    private static List<DirectoryInfo> GetAllSaveFolders()
    {
        var folders = SavingSystem.GetFoldersInDirectory();
        var saveFolders = new List<DirectoryInfo>();

        foreach (var item in folders)
        {
            if (item.Name.StartsWith(saveDirectory))
                saveFolders.Add(item);
        }

        return saveFolders;
    }

}

public class SaveSlotBaseData
{
    public int id;
    public string name;
    public float completionPercentage;
    public float playTime; //Play time in seconds

    public SaveSlotBaseData()
    {
        id = 1;
        name = "Default";
        completionPercentage = 0f;
        playTime = 0f;
    }

    public SaveSlotBaseData(int id, string name, float completionPercentage, float playTime)
    {
        this.id = id;
        this.name = name;
        this.completionPercentage = completionPercentage;
        this.playTime = playTime;
    }
}