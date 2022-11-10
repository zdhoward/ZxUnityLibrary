using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    [Header("SaveLoad Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler fileDataHandler;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of SaveLoadManager in this scene!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadGame("autosave");
            Debug.Log("Loading Game");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame("autosave");
            Debug.Log("Saving Game");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NewGame();
            Debug.Log("New Game");
        }
    }

    void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, "saves", fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame("autosave");
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame(string fileName)
    {
        this.gameData = fileDataHandler.Load(fileName + ".json");

        if (this.gameData == null)
        {
            Debug.Log("No data found. Starting New Game.");
            NewGame();
        }

        foreach (IDataPersistence item in dataPersistenceObjects)
        {
            item.LoadData(gameData);
        }
    }

    public void SaveGame(string fileName)
    {
        foreach (IDataPersistence item in dataPersistenceObjects)
        {
            item.SaveData(ref gameData);
        }

        fileDataHandler.Save(fileName + ".json", gameData);
    }

    public void ContinueGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles("*.json").OrderBy(f => f.LastWriteTime).Reverse().ToArray<FileInfo>();

        LoadGame(files[0].Name);
    }

    private void OnApplicationQuit()
    {
        SaveGame("autosave");
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
