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

    public static MenuUIManager Instance { get; private set; }

    public Button MissionBtnPrefab => missionBtnPrefab;
    public Transform MissionBtnParent => missionBtnParent;
    public GameObject UnitSelectPanel => unitSelectPanel;
    public GameObject MissionText => missionText;
    public GameObject MissionPanel => missionPanel;
    public GameObject MissionList => missionList;
    public Button BackBtn => backBtn;
    public Button DeleteDataBtn => deleteDataBtn;

    void Awake() {
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            missionBtnPrefab.onClick.AddListener(() => {
                unitSelectPanel.SetActive(true);
                missionText.SetActive(false);
                missionPanel.SetActive(false);
            });
        }
    }
}
