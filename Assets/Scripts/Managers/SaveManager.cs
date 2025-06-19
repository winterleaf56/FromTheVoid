using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

[System.Serializable]
public class LevelData {
    public string levelName;
    public bool levelCompleted;
}

[System.Serializable]
public class SaveData {
    public List<LevelData> levels = new List<LevelData>();
}

public class SaveManager : MonoBehaviour {

    private Button deleteDataBtn;

    public Action SaveGame;

    private string saveFilePath;
    private List<Level> levels;

    public static SaveManager Instance { get; private set; }

    private void Awake() {
        // This ensures that the SaveManager object persists across scene loads (Probably not needed)
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Sets the SaveGame action to the SaveGameData method and the sceneLoaded event to the OnSceneLoaded method (Runs when scene loads)
        SaveGame += SaveGameData;
        SceneManager.sceneLoaded += OnSceneLoaded;

        saveFilePath = Path.Combine(Application.dataPath, "SaveData.json");
    }

    private void Start() {
        // Loads game data on start
        Debug.Log("SaveManager Start called. Loading game data.");
        LoadGameData();
    }

    private void OnDestroy() {
        // Unsubscribe from the SaveGame action and sceneLoaded event to prevent memory leaks
        SaveGame -= SaveGameData;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // When MainMenue is loaded, sets the deleteDataBtn and adds DeleteAllData listener to it
        if (scene.name == "MainMenu") {
            Debug.Log("MainMenu scene loaded. Loading levels and game data.");

            deleteDataBtn = MenuUIManager.Instance.DeleteDataBtn;
            deleteDataBtn.onClick.AddListener(DeleteAllData);
        }

        // Not sure if this will cause issues down the line but loads levels from the LevelManager instance. (Currently does this on any scene load)
        if (LevelManager.Instance != null) {
            levels = LevelManager.Instance.GetLevels();
        } else {
            Debug.LogError("LevelManager instance is not available. Cannot load levels.");
        }
    }

    private void SaveGameData() {
        SaveData saveData = new SaveData();

        foreach (Level level in levels) {
            LevelData levelData = new LevelData {
                levelName = level.LevelName,
                levelCompleted = level.LevelCompleted
            };

            Debug.Log($"Saving level: {levelData.levelName}, Completed: {levelData.levelCompleted}");
            saveData.levels.Add(levelData);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game data saved to {saveFilePath}");
    }

    public void LoadGameData() {
        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            foreach (LevelData levelData in saveData.levels) {
                Level level = levels.Find(l => l.LevelName == levelData.levelName);
                if (level != null) {
                    if (levelData.levelCompleted) {
                        level.ChangeCompletionStatus(true);
                        Debug.Log($"Level {levelData.levelName} is completed.");
                        LevelManager.Instance.levels.Find(l => l.LevelName == levelData.levelName).ChangeCompletionStatus(true);
                    }
                }
            }
            Debug.Log("Game data loaded successfully.");
        } else {
            Debug.LogWarning("Save file not found. No data loaded.");
        }
    }

    // Resets all data by changing each level's completion status to false, saves the game data, and loads the MainMenu scene.
    public void DeleteAllData() {
        if (File.Exists(saveFilePath)) {

            foreach (Level level in levels) {
                level.ChangeCompletionStatus(false);
            }

            SaveGameData();
            SceneManager.LoadScene("MainMenu");
        } else {
            Debug.LogWarning("No save file found to delete.");
        }
    }
}

/*
Leaving this in as future reference incase I need it.

[System.Serializable]
public class SaveManager : MonoBehaviour {
    public static bool Save(object saveData) {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/UserData")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/UserData");
        }

        string path = Application.persistentDataPath + "/UserData/SaveData.save";

        FileStream file = File.Create(path);
        formatter.Serialize(file, saveData);
        file.Close();

        return true;
    }

    public static object Load(string path) {
        if (!File.Exists(path)) {
            Debug.LogWarning("Save file not found at: " + path);
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        try {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        } catch (Exception e) {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter() {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}*/