using System;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class SavingExampleController : MonoBehaviour
{
    [Header("UI")]
    [Space(10)]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteSaveButton;
    [SerializeField] private Button deleteFolderButton;
    [Header("Position")]
    [SerializeField] private Slider positionX;
    [SerializeField] private Slider positionY;
    [SerializeField] private Slider positionZ;
    [Header("Rotation")]
    [SerializeField] private Slider rotationX;
    [SerializeField] private Slider rotationY;
    [SerializeField] private Slider rotationZ;
    [Header("Scale")]
    [SerializeField] private Slider scaleX;
    [SerializeField] private Slider scaleY;
    [SerializeField] private Slider scaleZ;
    [Header("Color")]
    [SerializeField] private Slider colorR;
    [SerializeField] private Slider colorG;
    [SerializeField] private Slider colorB;

    [Space(10)]
    [SerializeField] private GameObject cube;
    private Material cubeMaterial;

    private void Awake()
    {
        //Assigning button functions manually
        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(SaveCubeData);
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(LoadCubeData);
        deleteSaveButton.onClick.RemoveAllListeners();
        deleteSaveButton.onClick.AddListener(DeleteExampleData);
        deleteFolderButton.onClick.RemoveAllListeners();
        deleteFolderButton.onClick.AddListener(DeleteExampleFolder);

        //Assigning callbacks to when data has been loaded and saved
        SavingSystem.OnSave += OnDataSaved;
        SavingSystem.OnLoad += OnDataLoaded;

        //Caching the cube material
        cubeMaterial = cube.GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        var position = new Vector3(positionX.value, positionY.value, positionZ.value);
        cube.transform.position = position;

        var rotation = Quaternion.Euler(rotationX.value, rotationY.value, rotationZ.value);
        cube.transform.rotation = rotation;

        var scale = new Vector3(scaleX.value, scaleY.value, scaleZ.value);
        cube.transform.localScale = scale;

        var color = new Color(colorR.value / 255, colorG.value / 255, colorB.value / 255);
        cubeMaterial.color = color;
    }

    private void OnDestroy()
    {
        SavingSystem.OnSave -= OnDataSaved;
        SavingSystem.OnLoad -= OnDataLoaded;
    }

    private void SaveCubeData()
    {
        //Creating cube data object and assigning cube values
        var data = new CubeExampleData();
        data.position = cube.transform.position;
        data.rotation = new Vector3(rotationX.value, rotationY.value, rotationZ.value);
        data.scale = cube.transform.localScale;
        data.color = cubeMaterial.color;

        if (SavingSystem.SaveToRoot(data, "SaveDataExample", "Example/"))
            Debug.Log("[SaveTest] Cube data saved to directory");
        else
            Debug.Log("[SaveTest] Cube data failed to save to directory");
    }

    private void LoadCubeData()
    {
        if (SavingSystem.SaveExistsOnRoot("SaveDataExample", "Example/"))
        {
            //Loading cube data from local directory
            var data = SavingSystem.LoadFromRoot<CubeExampleData>("SaveDataExample", "Example/");
            //Assigning saved cube data to UI elements
            positionX.value = data.position.x;
            positionY.value = data.position.y;
            positionZ.value = data.position.z;
            rotationX.value = data.rotation.x;
            rotationY.value = data.rotation.y;
            rotationZ.value = data.rotation.z;
            scaleX.value = data.scale.x;
            scaleY.value = data.scale.y;
            scaleZ.value = data.scale.z;
            colorR.value = data.color.r * 255;
            colorG.value = data.color.g * 255;
            colorB.value = data.color.b * 255;
        }
        else 
            Debug.LogWarning("[SaveTest] No file to load");
    }

    private void DeleteExampleData()
    {
        SavingSystem.DeleteSaveFileOnRoot("SaveDataExample", "Example/");
    }

    private void DeleteExampleFolder()
    {
        SavingSystem.DeleteAllSaveFiles("Example");
    }

    private void OnDataSaved(bool success)
    {
        if (success)
            Debug.Log("[SaveTest] SaveCallback: Data has been saved");
        else
            Debug.Log("[SaveTest] SaveCallback: Data has failed to be saved");
    }

    private void OnDataLoaded(bool success)
    {
        if (success)
            Debug.Log("[SaveTest] LoadCallback: Data has been loaded");
        else
            Debug.Log("[SaveTest] LoadCallback: Data has failed to be loaded");
    }

    [Serializable]
    public class CubeExampleData
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Color color;
    }
}
#pragma warning restore 0649