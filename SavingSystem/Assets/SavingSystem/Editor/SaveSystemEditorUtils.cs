using UnityEditor;
using UnityEngine;

public class SaveSystemEditorUtils : MonoBehaviour
{
    [MenuItem("Save System/Delete Save Files")]
    static void DeleteSaveFiles()
    {
        if (EditorUtility.DisplayDialog("Save files deletion", "Are you sure you want to delete the save files? This is irreversible.", "Confirm", "Cancel"))
            SaveFileSystem.DeleteSaveFiles();
    }
}