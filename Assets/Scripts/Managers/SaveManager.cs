using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[System.Serializable]
public class LevelData {
    public string levelName;
    public bool levelCompleted;
    public bool firstCompleted;
}

[System.Serializable]
public class FriendlyUnitData {
    public int friendlyID;
    public string unitName;
    public int actionPoints;
    public int actionPointRecovery;
    //public int ultimatePoints;
    public int maxUltimatePoints;
    public float maxHealth;
    public float maxMoveDistance;
    public float defence;
    public List<MoveData> moves = new List<MoveData>();
    public List<MoveData> actions = new List<MoveData>();
    public int duplicates;
}

[System.Serializable]
public class MoveData {
    public string moveName;
    public float value; // Damage, Heal, etc.
    public int actionPointCost;
    public float range;
    public bool isAOE;
}

[System.Serializable]
public class SaveData {
    public List<LevelData> levels = new List<LevelData>();

    public string playerName;
    //public int playerId;
    public int currentLevel;
    public int currentVoidShards;
    public int currentCoins;

    public List<FriendlyUnitData> playerUnits = new List<FriendlyUnitData>();
}

public class SaveManager : MonoBehaviour {

    private Button deleteDataBtn;

    private SaveData cachedSaveData;

    public Action SaveGame;
    public Action LoadGame;
    public Action saveNewUnit;

    private string saveFilePath;
    private List<Level> levels;

    public static SaveManager Instance { get; private set; }

    /*private void Awake() {
        //LoadGameData();

        // This ensures that the SaveManager object persists across scene loads (Probably not needed, good for saving games in progress)
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Sets the SaveGame action to the SaveGameData method and the sceneLoaded event to the OnSceneLoaded method (Runs when scene loads)
        SaveGame += SaveGameData;
        LoadGame += LoadGameData;
        //saveNewUnit += SaveNewUnit;
        SceneManager.sceneLoaded += OnSceneLoaded;

        saveFilePath = Path.Combine(Application.dataPath, "SaveData.json");
        LoadGameData();
    }*/

    private void Awake() {
        // Save inside the game bundle / project Assets folder (original behaviour)
        saveFilePath = Path.Combine(Application.dataPath, "SaveData.json");
        Debug.Log($"Save file path: {saveFilePath}");

        // Keep this object across scenes
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Wire up save/load events and scene callback
        SaveGame += SaveGameData;
        LoadGame += LoadGameData;
        //saveNewUnit += SaveNewUnit;
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Ensure folder exists (Application.dataPath typically exists but be safe)
        var dir = Path.GetDirectoryName(saveFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }

        // Load save now that path is set
        LoadGameData();
    }

    private void Start() {
        // Loads game data on start
        Debug.Log("SaveManager Start called. Loading game data.");
        //LoadGameData();
    }

    private void OnDestroy() {
        // Unsubscribe from the SaveGame action and sceneLoaded event to prevent memory leaks
        SaveGame -= SaveGameData;
        LoadGame -= LoadGameData;
        //saveNewUnit -= SaveNewUnit;
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
            ApplyLevelCompletionData();
        } else {
            Debug.LogError("LevelManager instance is not available. Cannot load levels.");
        }
    }

    // Call this from OnSceneLoaded after levels is set
    private void ApplyLevelCompletionData() {
        if (levels == null || cachedSaveData == null) return;
        foreach (LevelData levelData in cachedSaveData.levels) {
            Level level = levels.Find(l => l.LevelName == levelData.levelName);
            if (level != null) {
                LevelManager.Instance.levels.Find(l => l.LevelName == levelData.levelName).ChangeFirstCompletion(levelData.firstCompleted);
                LevelManager.Instance.levels.Find(l => l.LevelName == levelData.levelName).ChangeCompletionStatus(levelData.levelCompleted);
            }
        }
    }

    // Make seperate methods for all save actions, then run them all in SaveGameData.
    private void SaveGameData() {
        SaveData saveData = new SaveData();

        // Save level completion data
        foreach (Level level in levels) {
            LevelData levelData = new LevelData {
                levelName = level.LevelName,
                levelCompleted = level.LevelCompleted,
                firstCompleted = level.FirstCompleted
            };

            Debug.Log($"Saving level: {levelData.levelName}, Completed: {levelData.levelCompleted}");
            saveData.levels.Add(levelData);
        }

        // Save player details
        if (PlayerDetailsManager.Instance != null) {
            Debug.Log($"Saving player details: Name: {PlayerDetailsManager.Instance.PlayerName}, Void Shards: {PlayerDetailsManager.Instance.GetCurrentVoidShards()}, Coins: {PlayerDetailsManager.Instance.GetCurrentCoins()}");
            saveData.playerName = PlayerDetailsManager.Instance.PlayerName;
            saveData.currentVoidShards = PlayerDetailsManager.Instance.GetCurrentVoidShards();
            saveData.currentCoins = PlayerDetailsManager.Instance.GetCurrentCoins();
        } else {
            Debug.LogWarning("PlayerDetailsManager instance is not available. Player details will not be saved.");
        }

        // Save player's units
        if (PlayerDetailsManager.Instance != null && PlayerDetailsManager.Instance.playerUnits != null) {
            foreach (Friendly unit in PlayerDetailsManager.Instance.playerUnits) {
                if (unit != null) {
                    FriendlyUnitData data = new FriendlyUnitData() {
                        friendlyID = unit.FriendlyUnitID,
                        unitName = unit.UnitStats.UnitName,
                        actionPoints = unit.UnitStats.ActionPoints,
                        actionPointRecovery = unit.UnitStats.ActionPointRecovery,
                        maxUltimatePoints = unit.UnitStats.MaxUltimatePoints,
                        maxHealth = unit.UnitStats.MaxHealth,
                        maxMoveDistance = unit.UnitStats.MaxMoveDistance,
                        defence = unit.UnitStats.Defence,
                        duplicates = unit.UnitStats.Duplicates
                    };
                    saveData.playerUnits.Add(data);
                }
            }
        }

        //saveData.playerUnits.Clear();
        /*List<Friendly> playerUnits = PlayerDetailsManager.Instance.playerUnits;
        
        if (playerUnits.Count != 0 && playerUnits != null) {
            foreach (Friendly unit in playerUnits) {
                if (unit != null) {
                    saveData.playerUnits.Add(unit);
                    Debug.Log($"Saving unit: {unit.name}");
                }
            }
        } else {
            Debug.LogWarning("No player units found to save. Player units list is empty or null.");
        }*/

        /*var playerUnits = PlayerDetailsManager.Instance.playerUnits;
        foreach (Friendly unit in playerUnits) {
            if (unit != null) {
                var data = new FriendlyUnitData() {
                    friendlyID = unit.FriendlyUnitID,
                    unitName = unit.UnitStats.UnitName,
                    actionPoints = unit.UnitStats.ActionPoints,
                    actionPointRecovery = unit.UnitStats.ActionPointRecovery,
                    maxUltimatePoints = unit.UnitStats.MaxUltimatePoints,
                    maxHealth = unit.UnitStats.MaxHealth,
                    maxMoveDistance = unit.UnitStats.MaxMoveDistance,
                    defence = unit.UnitStats.Defence,
                    moves = new List<ActionBase>(),
                    actions = new List<ActionBase>(),
                    duplicates = unit.Duplicates
                };
                saveData.playerUnits.Add(data);
            }
        }*/


        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game data saved to {saveFilePath}");
    }

    public void SaveLevelData(Level level) {
        if (level == null) {
            Debug.LogWarning("Level is null. Cannot save level data.");
            return;
        }

        // Load existing save data
        SaveData saveData = File.Exists(saveFilePath)
            ? JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath))
            : new SaveData();

        // Find or update the level data
        LevelData levelData = saveData.levels.Find(l => l.levelName == level.LevelName);
        if (levelData == null) {
            levelData = new LevelData { levelName = level.LevelName };
            saveData.levels.Add(levelData);
        }

        levelData.levelCompleted = level.LevelCompleted;
        levelData.firstCompleted = level.FirstCompleted;

        // Save updated data back to the file
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Level {level.LevelName} data saved.");
    }

    public void SavePlayerData() {
        // Load existing data so you don’t overwrite the level info
        SaveData saveData = File.Exists(saveFilePath)
            ? JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath))
            : new SaveData();

        if (PlayerDetailsManager.Instance != null) {
            saveData.playerName = PlayerDetailsManager.Instance.PlayerName;
            saveData.currentVoidShards = PlayerDetailsManager.Instance.GetCurrentVoidShards();
            saveData.currentCoins = PlayerDetailsManager.Instance.GetCurrentCoins();
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Player data saved to {saveFilePath}");
    }

    /*private void SaveNewUnit() {
        SaveData saveData = File.Exists(saveFilePath)
            ? JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath))
            : new SaveData();

        var playerUnits = PlayerDetailsManager.Instance.playerUnits;
        foreach (Friendly unit in playerUnits) {
            if (unit != null) {
                var data = new FriendlyUnitData() {
                    friendlyID = unit.FriendlyUnitID,
                    unitName = unit.UnitStats.UnitName,
                    actionPoints = unit.UnitStats.ActionPoints,
                    actionPointRecovery = unit.UnitStats.ActionPointRecovery,
                    maxUltimatePoints = unit.UnitStats.MaxUltimatePoints,
                    maxHealth = unit.UnitStats.MaxHealth,
                    maxMoveDistance = unit.UnitStats.MaxMoveDistance,
                    defence = unit.UnitStats.Defence,
                    moves = new List<ActionBase>(),
                    actions = new List<ActionBase>(),
                    duplicates = unit.Duplicates
                };
                saveData.playerUnits.Add(data);
            }
        }

        *//*string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);*//*
        Save(saveData);
        Debug.Log($"New unit data saved to {saveFilePath}");
    }*/

    private void Save(SaveData data) {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGameData() {
        /* if (File.Exists(saveFilePath)) {
             string json = File.ReadAllText(saveFilePath);
             Debug.Log("Raw JSON data loaded: " + json);
             SaveData saveData = JsonUtility.FromJson<SaveData>(json);


             // Load level completion data
             foreach (LevelData levelData in saveData.levels) {
                 Level level = levels.Find(l => l.LevelName == levelData.levelName);
                 if (level != null) {
                     LevelManager.Instance.levels.Find(l => l.LevelName == levelData.levelName).ChangeFirstCompletion(levelData.firstCompleted);
                     LevelManager.Instance.levels.Find(l => l.LevelName == levelData.levelName).ChangeCompletionStatus(levelData.levelCompleted);
                 }
             }

             // Load player details
             if (PlayerDetailsManager.Instance != null) {
                 Debug.Log($"Loading player name: {saveData.playerName}, Void Shards: {saveData.currentVoidShards}, Coins: {saveData.currentCoins}");
                 PlayerDetailsManager.Instance.SetPlayerName(saveData.playerName);
                 //PlayerDetailsManager.Instance.AddVoidShards(saveData.currentVoidShards - PlayerDetailsManager.Instance.GetCurrentVoidShards());
                 //PlayerDetailsManager.Instance.AddCoins(saveData.currentCoins - PlayerDetailsManager.Instance.GetCurrentCoins());
                 PlayerDetailsManager.Instance.SetVoidShards(saveData.currentVoidShards);
                 PlayerDetailsManager.Instance.SetCoins(saveData.currentCoins);
             } else {
                 Debug.LogWarning("PlayerDetailsManager instance is not available. Player details will not be loaded.");
             }*/
        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            Debug.Log("Raw JSON data loaded: " + json);
            cachedSaveData = JsonUtility.FromJson<SaveData>(json);

            // Load player details
            if (PlayerDetailsManager.Instance != null) {
                Debug.Log($"Loading player name: {cachedSaveData.playerName}, Void Shards: {cachedSaveData.currentVoidShards}, Coins: {cachedSaveData.currentCoins}");
                PlayerDetailsManager.Instance.SetPlayerName(cachedSaveData.playerName, false);
                PlayerDetailsManager.Instance.SetVoidShards(cachedSaveData.currentVoidShards);
                PlayerDetailsManager.Instance.SetCoins(cachedSaveData.currentCoins);
            } else {
                Debug.LogWarning("PlayerDetailsManager instance is not available. Player details will not be loaded.");
            }

            // Load player's units
            if (PlayerDetailsManager.Instance != null && PlayerDetailsManager.Instance.UnitDatabase != null) {
                var loadedUnits = new List<Friendly>();
                foreach (FriendlyUnitData unitData in cachedSaveData.playerUnits) {
                    var prefab = PlayerDetailsManager.Instance.UnitDatabase.GetUnitPrefabByID(unitData.friendlyID);
                    if (prefab == null) {
                        Debug.LogWarning($"No prefab found for unit {unitData.unitName}");
                        continue;
                    }

                    var unitObj = Instantiate(prefab);
                    var friendly = unitObj.GetComponent<Friendly>();

                    // Restore runtime stats using setters
                    friendly.stats.SetActionPoints(unitData.actionPoints);
                    friendly.stats.AddActionPointRecovery(unitData.actionPointRecovery - friendly.stats.ActionPointRecovery);
                    friendly.stats.AddUltimatePoints(unitData.maxUltimatePoints - friendly.stats.MaxUltimatePoints);
                    friendly.stats.SetMaxMoveDistance(unitData.maxMoveDistance);
                    friendly.stats.SetDefence(unitData.defence);
                    // If you have a health component, set current health here
                    friendly.stats.SetMaxHealth(unitData.maxHealth);
                    friendly.stats.SetDuplicates(unitData.duplicates);

                    loadedUnits.Add(friendly);
                }
                PlayerDetailsManager.Instance.LoadUnits(loadedUnits);
            } else {
                Debug.LogError("PlayerDetailsManager instance or UnitDatabase is not available. Player units will not be loaded.");
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

            PlayerDetailsManager.Instance.SetPlayerName("", false);
            PlayerDetailsManager.Instance.SetVoidShards(0);
            PlayerDetailsManager.Instance.SetCoins(0);

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