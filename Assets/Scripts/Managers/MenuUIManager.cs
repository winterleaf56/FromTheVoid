using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour {

    [SerializeField] private Button missionBtnPrefab;
    [SerializeField] private Transform missionBtnParent;
    [SerializeField] private GameObject unitSelectPanel;
    [SerializeField] private GameObject missionText;
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private GameObject missionList;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button deleteDataBtn;
    [SerializeField] private GameObject mainMenuPanel;

    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Button submitUsernameBtn;

    [Header("Main Menu User Details")]
    [SerializeField] private TMP_Text usernameDisplayTxt;
    [SerializeField] private TMP_Text userRankDisplayTxt;
    [SerializeField] private TMP_Text userVoidShardDisplayTxt;
    [SerializeField] private TMP_Text userCoinDisplayTxt;

    public static Action updateUserDetails;

    public static MenuUIManager Instance { get; private set; }

    public Button MissionBtnPrefab => missionBtnPrefab;
    public Transform MissionBtnParent => missionBtnParent;
    public GameObject UnitSelectPanel => unitSelectPanel;
    public GameObject MissionText => missionText;
    public GameObject MissionPanel => missionPanel;
    public GameObject MissionList => missionList;
    public Button BackBtn => backBtn;
    public Button DeleteDataBtn => deleteDataBtn;
    public GameObject UsernamePanel => usernamePanel;
    public TMP_InputField UsernameInput => usernameInput;
    public Button SubmitUsernameBtn => submitUsernameBtn;
    public GameObject MainMenuPanel => mainMenuPanel;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        updateUserDetails += UpdateUserDetails;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        updateUserDetails -= UpdateUserDetails;
    }

    // Updates user details in the main menu in the top bar
    private void UpdateUserDetails() {
        usernameDisplayTxt.SetText(PlayerDetailsManager.Instance.PlayerName);
        userRankDisplayTxt.SetText($"Rank: {PlayerDetailsManager.Instance.PlayerRank}");
        userVoidShardDisplayTxt.SetText(PlayerDetailsManager.Instance.GetCurrentVoidShards().ToString());
        userCoinDisplayTxt.SetText(PlayerDetailsManager.Instance.GetCurrentCoins().ToString());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            missionBtnPrefab.onClick.AddListener(() => {
                //unitSelectPanel.SetActive(true);
                missionText.SetActive(false);
                missionPanel.SetActive(false);
            });
        }
    }
}
