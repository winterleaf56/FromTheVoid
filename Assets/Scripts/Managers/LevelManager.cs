using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [SerializeField] public List<Level> levels = new List<Level>();

    //private MenuUIManager menuUIManager = MenuUIManager.Instance;

    private Button missionBtnPrefab;
    private Transform missionBtnParent;
    private GameObject unitSelectPanel;
    private GameObject missionText;
    //private GameObject missionPanel;
    private GameObject missionList;
    private Button backBtn;

    public static LevelManager Instance;

    void Awake() {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void CacheUIRefs() {
        var menuUIManager = MenuUIManager.Instance;
        if (menuUIManager != null) {
            missionBtnPrefab = menuUIManager.MissionBtnPrefab;
            missionBtnParent = menuUIManager.MissionBtnParent;
            unitSelectPanel = menuUIManager.UnitSelectPanel;
            missionText = menuUIManager.MissionText;
            //missionPanel = menuUIManager.MissionPanel;
            missionList = menuUIManager.MissionList;
            backBtn = menuUIManager.BackBtn;
        } else {
            Debug.LogError("MenuUIManager.Instance is null!");
        }
    }

    private void Start() {
        //SaveManager.Instance.SaveGame.Invoke();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            CacheUIRefs();
            SaveManager.Instance.LoadGameData();
            LoadLevels();
        }
    }

    public void LoadLevels() {
        try {
            foreach (Button button in missionBtnParent.GetComponentsInChildren<Button>()) {
                Destroy(button.gameObject);
            }
        } catch (System.Exception e) {
            Debug.LogError("Error clearing mission buttons: " + e.Message);
        }

        foreach (Level level in levels) {
            //level.LoadCompletionStatus();
            Debug.Log("Creating button for: " + level.LevelName);
            CreateMissionButton(level);
        }
    }

    private void CreateMissionButton(Level level) {
        Button missionBtn = Instantiate(missionBtnPrefab, missionBtnParent);
        missionBtn.GetComponentInChildren<TMP_Text>().text = level.LevelName;
        missionBtn.onClick.RemoveAllListeners();

        if (level.LevelCompleted) {
            missionBtn.transform.Find("CompletedImage").gameObject.SetActive(true);
        }

        missionBtn.onClick.AddListener(() => {
            GameManager.Instance.SetSelectedLevel(level);
            GameManager.Instance.LoadUnitButtons();
            unitSelectPanel.SetActive(true);
            level.ClearUnits();

            missionText.SetActive(false);
            missionList.SetActive(false);
            backBtn.gameObject.SetActive(false);
        });
    }

    // Returns the list of levels (Used in SaveManager to save levels)
    public List<Level> GetLevels() {
        return levels;
    }
}
