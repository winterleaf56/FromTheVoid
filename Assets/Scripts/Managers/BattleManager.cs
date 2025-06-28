using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    [Header("Display Selected Unit Stats")]
    [SerializeField] private GameObject unitStatsPanel;
    [SerializeField] private TMP_Text healthValTxt;
    [SerializeField] private TMP_Text damageValTxt;
    [SerializeField] private TMP_Text actionPointsValTxt;

    [Header("UI Elements")]
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject turnUI;
    [SerializeField] private GameObject battleActionsUI;
    [SerializeField] private GameObject unitMovesUI;
    [SerializeField] private GameObject unitActionUI;
    [SerializeField] private GameObject battleAttackBtnUI;
    [SerializeField] private GameObject unitStatUI;
    [SerializeField] private TMP_Text roundNumber;

    [Header("Lighting")]
    [SerializeField] private GameObject selectingEnemyLight;
    [SerializeField] private GameObject worldLight;

    [Header("Unit References")]
    [SerializeField] private GameObject friendlyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] friendlySpawns;
    [SerializeField] private GameObject[] enemySpawns;

    [SerializeField] private List<Friendly> deadFriendlyUnits;
    [SerializeField] private List<Enemy> deadEnemyUnits;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private CameraController customCamera;

    private bool paused = false;

    public List<Enemy> DeadEnemyUnits { get => deadEnemyUnits; }

    //[SerializeField] private Level Level;

    public static Level SelectedLevel { get; set; }

    //public GameObject[] playerUnits;
    public List<GameObject> playerUnits;
    public List<GameObject> enemyUnits;

    public int turnNumber { get; private set; }
    public int waveNumber { get; private set; } // Waves not implemented, add at later time

    public float globalActionPoints { get; private set; }
    public float increaseGAPBy { get; private set; } = 50;

    public static Action onPlayerTurnStart;
    public static Action<BattleState> changeBattleState;
    public static Action onMoveFinished;
    public static Action<List<Enemy>> manageLights;
    
    public static Action<bool> onGamePaused;

    public static Action<Lifeforms> unitDied;
    //public static Action enemyUnitDied;

    //public delegate void PlayAudioClipDelegate(AudioClip clip, Vector3 position);
    public static Action<AudioClip, Vector3> audioClip;
    //public event PlayAudioClipDelegate audioClip;

    public static BattleManager Instance;

    public enum GameState {
        Intro,
        PlayerTurn,
        EnemyTurn,
        BattleOver
    }

    public enum BattleState {
        PlayerIdle,
        PlayerAttack,
        PlayerDefend,
        PlayerItem,
        PlayerAction,
        PlayerMoving
    }

    //public GameState currentTurn { get; private set; }


    // Need to decide: Reset UI here, or in PlayerTurn script
    private GameState _currentTurn;
    public GameState currentTurn {
        get => _currentTurn;
        private set { 
            _currentTurn = value;
            if (_currentTurn == GameState.PlayerTurn) {
                //battleActionsUI.SetActive(true);
                print("Player turn started");
            } else if (_currentTurn == GameState.EnemyTurn) {
                //battleActionsUI.SetActive(false);
                print("Enemy turn started");
            }
        }
    }

    // Updated during PlayerTurn to differentiate between different actions for clicks, lighting changes, etc
    private BattleState _currentBattleState;
    public BattleState currentBattleState { 
        get => _currentBattleState; 
        private set {
            _currentBattleState = value;
            Debug.Log($"Current Battle State now set to: {_currentBattleState}");
            if (_currentBattleState == BattleState.PlayerAttack) {
                ToggleWorldLight(false);
            } else {
                ToggleWorldLight(true);
            }
            
            //ToggleWorldLight();
        } 
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        deadEnemyUnits = new List<Enemy>();

        changeBattleState += ChangeBattleState;
        onMoveFinished += OnMoveFinshed;
        manageLights += ManageLights;
        unitDied += UnitDied;

        audioClip += (clip, position) => {
            //audioSource.clip = clip;
            AudioSource.PlayClipAtPoint(clip, position);
        };
        //PlayerTurn.Instance.playerTurnEnded += ClearPlayerTurn;

        GameObject prefab = SelectedLevel.EnvironmentPrefab;
        GameObject environmentInstance = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation); // Might need to change this or perhaps need to change camera position instead
        EnvironmentBridge bridge = environmentInstance.GetComponent<EnvironmentBridge>();

        friendlySpawns = bridge.friendlySpawns;
        enemySpawns = bridge.enemySpawns;
        customCamera.boundaryObject = bridge.cameraBoundary;

        currentTurn = GameState.Intro;

        //currentTurn = GameState.PlayerTurn;
    }

    private void OnDestroy() {
        changeBattleState -= ChangeBattleState;
        onMoveFinished -= OnMoveFinshed;
        manageLights -= ManageLights;
        Debug.Log("Battle Manager destroyed");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Escape Pressed");
            PauseGame();
        }
    }

    /*public void PlayAudioClip(AudioClip clip, Vector3 position) {
        AudioSource.PlayClipAtPoint(clip, position);
    }*/

    void Start() {

        for (int i = 0; i < SelectedLevel.PlayerUnits.Count; i++) {
            GameObject unit = Instantiate(SelectedLevel.PlayerUnits[i], friendlySpawns[i].transform.position, Quaternion.Euler(0, -90, 0));
            playerUnits.Add(unit);
        }

        for (int i = 0; i < enemySpawns.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawns[i].transform.position, Quaternion.Euler(0, 90, 0));
            enemyUnits.Add(enemy);
        }

        currentTurn = GameState.PlayerTurn;

        PlayerTurn.Instance.playerTurnEnded += ClearPlayerTurn;
        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle() {
        while (!currentTurn.Equals(GameState.BattleOver)) {
            turnNumber++;

            Debug.Log("Battle Manager: Starting Player Turn");
            currentTurn = GameState.PlayerTurn;

            yield return null;
            // Check objectives before player turn starts
            CheckObjectives();

            foreach (GameObject unit in playerUnits) {
                if (unit != null) {
                    Debug.Log($"GETTING UNIT: {unit} IN PLAYERUNITS");
                    Lifeforms friendlyUnit = unit.GetComponent<Friendly>();
                    friendlyUnit.StartRound();
                }
            }

            // Runs PlayerTurn logic and waits for the turn to end
            yield return StartCoroutine(GetComponent<PlayerTurn>().StartPlayerTurn());
            Debug.Log("Battle Manager: Ending Player Turn");

            Debug.Log("Battle Manager: Starting Enemy Turn");
            currentTurn = GameState.EnemyTurn;

            // Check objectives before enemy turn starts
            CheckObjectives();

            foreach (GameObject enemy in enemyUnits) {
                if (enemy != null) {
                    Debug.Log($"GETTING UNIT: {enemy} IN ENEMYUNITS");
                    Lifeforms enemyUnit = enemy.GetComponent<Enemy>();
                    enemyUnit.StartRound();
                }
            }

            yield return StartCoroutine(GetComponent<EnemyTurn>().StartEnemyTurn(enemyUnits));
            Debug.Log("Battle Manager: Ending Enemy Turn");
            
            yield return new WaitForSeconds(1);
        }
    }

    // I think I can just search the list for the unit that died, and remove it from the list. Change this later
    private void UnitDied(Lifeforms unit) {
        // Wrote quick switch statement to handle different unit types. If I want to move forward with this approach then this should work
        /*switch (unit) {
            case Friendly friendlyUnit:
                Debug.Log($"Friendly unit died: {friendlyUnit.stats.UnitName}");
                PlayerUnitDied();
                break;
            case Enemy enemyUnit:
                Debug.Log($"Enemy unit died: {enemyUnit.stats.UnitName}");
                EnemyUnitDied();
                break;
            default:
                Debug.LogWarning("Unknown unit type died");
                return;
        }*/

        if (unit.GetComponent<Friendly>()) {
            for (int i = 0; i < playerUnits.Count; i++) {
                if (playerUnits[i] == unit.gameObject) {
                    deadFriendlyUnits.Add(unit.GetComponent<Friendly>());
                    playerUnits[i] = null;
                    break;
                }
            }
        } else if (unit.GetComponent<Enemy>()) {
            for (int i = 0; i < enemyUnits.Count; i++) {
                if (enemyUnits[i] == unit.gameObject) {
                    deadEnemyUnits.Add(unit.GetComponent<Enemy>());
                    enemyUnits[i] = null;
                    break;
                }
            }
        }

        UIManager.updateObjectiveText(unit);

        AllUnitsDied();
        CheckObjectives();
    }

    // Change deadEnemyUnits.Count from 4 to what the objective value is. Or if there is a different objective, figure that out later
    // Changed but need to have a seperate GameOver for if all enemies die but objectives are not complete
    private void AllUnitsDied() {
        if (deadFriendlyUnits.Count == SelectedLevel.RequiredNumberOfUnits) {
            GameOver();
        }
    }

    public void DevModeVictory() {
        Victory();
    }

    private void CheckObjectives() {
        foreach (ObjectiveBase objective in SelectedLevel.Objectives) {
            if (!objective.isCompleted) {
                objective.CheckObjective();

                if (objective.isCompleted) {
                    Debug.Log($"Objective completed: {objective.objectiveText}");
                    UIManager.Instance.UpdateObjectiveUI(objective); // Update the UI
                }

                if (objective.isCompleted && AllObjectivesCompleted()) {
                    Debug.Log("All objectives completed!");
                    Victory(); // Call victory if all objectives are completed
                }
            }
        }
    }

    // Returns true if all objectives are completed
    private bool AllObjectivesCompleted() {
        foreach (ObjectiveBase objective in SelectedLevel.Objectives) {
            if (!objective.isCompleted) {
                return false;
            }
        }
        return true;
    }

    private void PlayerUnitDied() {

    }

    private void EnemyUnitDied() {

    }

    public void PauseGame() {
        if (!paused) {
            paused = true;
            onGamePaused?.Invoke(true);
            Time.timeScale = 0;
        } else {
            paused = false;
            onGamePaused?.Invoke(false);
            Time.timeScale = 1;
        }
    }

    private void GameOver() {
        Debug.Log("Game Over");
        UIManager.Instance.ShowDefeatPanel();
        currentTurn = GameState.BattleOver;
    }

    private void Victory() {
        Debug.Log("Victory");
        UIManager.Instance.ShowVictoryPanel();
        // This is here because currently we only give rewards for the first time a level is completed
        if (!SelectedLevel.FirstCompleted) {
            UIManager.updateRewardPanel(SelectedLevel.Rewards);
        }

        SelectedLevel.completedLevel?.Invoke(); // Invoke the completed level action
        //SelectedLevel.ChangeCompletionStatus(true);
        currentTurn = GameState.BattleOver;
    }

    // Possibly Not Needed / Replaced by StartBattle IEnumerator
    public void EndTurn() {
        SwitchTurns();
    }

    void SwitchTurns() {
        Debug.Log("Switching turns");

        currentTurn = (currentTurn == GameState.PlayerTurn) ? GameState.EnemyTurn : GameState.PlayerTurn;

        if (currentTurn == GameState.PlayerTurn) {
            onPlayerTurnStart?.Invoke();
        }

        Debug.Log($"Current turn: {currentTurn}");
    }

    public void UnitClicked(bool friendly) {
        if (currentBattleState == BattleState.PlayerAttack) {

        }
        if (friendly && currentTurn == GameState.PlayerTurn) {
            battleActionsUI.SetActive(true);
        } else {
            battleActionsUI.SetActive(false);
        }
    }

    public void ShowUnitStats(float health, float damage, float AP) {
        healthValTxt.SetText(health.ToString());
        damageValTxt.SetText(damage.ToString());
        actionPointsValTxt.SetText(AP.ToString());

        unitStatsPanel.SetActive(true);
    }

    public void HideUnitStats() {
        unitStatsPanel.SetActive(false);
    }

    private void ChangeBattleState(BattleState state) {
        currentBattleState = state;
        Debug.Log($"Changing state to: {currentBattleState}");
    }

    public void ManageLights(List<Enemy> enemyList) {
        foreach (Enemy enemy in enemyList) {
            Light displayLight = enemy.GetComponent<Light>();
            displayLight.color = Color.green;
            displayLight.enabled = !displayLight.enabled;
        }
    }


    private void ToggleWorldLight(bool value) {
        /*selectingEnemyLight.SetActive(!worldLight.activeSelf);
        worldLight.SetActive(!worldLight.activeSelf);*/

        /*selectingEnemyLight.SetActive(!value);
        worldLight.SetActive(value);*/

        Debug.Log($"WORLD LIGHT TOGGLE VALUE: {value}");

        if (value) {
            selectingEnemyLight.SetActive(false);
            worldLight.SetActive(true);
        } else {
            selectingEnemyLight.SetActive(true);
            worldLight.SetActive(false);
        }
    }

    public void OnMoveFinshed() {
        currentBattleState = BattleState.PlayerIdle;

        unitActionUI.SetActive(false);
        unitMovesUI.SetActive(false);
        battleActionsUI.SetActive(false);

        UIManager.Instance.ToggleStats(false);

        foreach (GameObject unit in playerUnits) {
            if (unit != null) {
                unit.GetComponent<Light>().enabled = false;
            }
        }
    }


    public void ClearPlayerTurn() {
        // Disable the lights on every player unit
        foreach (GameObject playerUnit in playerUnits) {
            if (playerUnit != null) {
                playerUnit.GetComponent<Light>().enabled = false;
            }
        }

        // Disable the lights on every enemy unit
        foreach (GameObject enemyUnit in enemyUnits) {
            if (enemyUnit != null) {
                enemyUnit.GetComponent<Light>().enabled = false;
            }
        }

    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
