using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private SettingsData settingsData;

    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one DataPersistanceManager in the scene!");
        }
        
        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        
        LoadGame();
    }

    public void NewGame()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        
        this.gameData = new GameData();
        //TODO - Make it so there is only one settings data object
        this.settingsData = new SettingsData();
    }

    public void SaveGame()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        
        foreach (IDataPersistance dataPersistanceObject in this.dataPersistanceObjects)
        {
            dataPersistanceObject.SaveGameData(ref this.gameData);
            //TODO - Make it so there is only one settings data object
            dataPersistanceObject.SaveSettingsData(ref this.settingsData);

        }

        Debug.Log("Game Saved!");

        dataHandler.SaveGameData(gameData);
        //TODO - Make it so there is only one settings data object
        dataHandler.SaveSettingsData(settingsData);

    }

    public void LoadGame()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        
        this.gameData = dataHandler.LoadGameData();
        //TODO - Make it so there is only one settings data object
        this.settingsData = dataHandler.LoadSettingsData();

        if (this.gameData == null)
        {
            Debug.LogError("No game data to load!");
            NewGame();
        }

        //TODO - Make it so there is only one settings data object
        if (this.settingsData == null)
        {
            Debug.LogError("No settings data to load!");
            NewGame();
        }


        foreach (IDataPersistance dataPersistanceObject in this.dataPersistanceObjects)
        {
            dataPersistanceObject.LoadGameData(this.gameData);
            //TODO - Make it so there is only one settings data object
            dataPersistanceObject.LoadSettingsData(this.settingsData);
        }

        Debug.Log("Game Loaded!");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }


}

