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

    public List<Enemy> DeadEnemyUnits { get => deadEnemyUnits; }

    //[SerializeField] private Level Level;

    public static Level SelectedLevel { get; set; }

    public GameObject[] playerUnits;
    public GameObject[] enemyUnits;

    public int turnNumber { get; private set; }
    public int waveNumber { get; private set; } // Waves not implemented, add at later time

    public float globalActionPoints { get; private set; }
    public float increaseGAPBy { get; private set; } = 50;

    public static Action onPlayerTurnStart;
    public static Action<BattleState> changeBattleState;
    public static Action onMoveFinished;
    public static Action<List<Enemy>> manageLights;

    public static Action<Lifeforms> unitDied;
    //public static Action enemyUnitDied;

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
                StartPlayerTurn();
                //battleActionsUI.SetActive(true);
                print("Player turn started");
            } else {
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

        changeBattleState += ChangeBattleState;
        onMoveFinished += OnMoveFinshed;
        manageLights += ManageLights;
        unitDied += UnitDied;
        //PlayerTurn.Instance.playerTurnEnded += ClearPlayerTurn;

        currentTurn = GameState.Intro;

        //currentTurn = GameState.PlayerTurn;
    }

    private void OnDestroy() {
        onPlayerTurnStart -= StartPlayerTurn;
        changeBattleState -= ChangeBattleState;
        onMoveFinished -= OnMoveFinshed;
        manageLights -= ManageLights;
        Debug.Log("Battle Manager destroyed");
    }

    [SerializeField] Canvas randomButtonCanvas;

    void Start() {
        enemyUnits = new GameObject[enemySpawns.Length];
        playerUnits = new GameObject[friendlySpawns.Length];

        for (int i = 0; i < SelectedLevel.PlayerUnits.Count; i++) {
            GameObject unit = Instantiate(SelectedLevel.PlayerUnits[i], friendlySpawns[i].transform.position, Quaternion.identity);
            playerUnits[i] = unit;
        }

        for (int i = 0; i < enemySpawns.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawns[i].transform.position, Quaternion.identity);
            enemyUnits[i] = enemy;
        }

        currentTurn = GameState.PlayerTurn;

        PlayerTurn.Instance.playerTurnEnded += ClearPlayerTurn;
        StartCoroutine(StartBattle());
    }

    private void StartPlayerTurn() {
        // Increases the global action points by the value of increaseGAPBy which can be changed in other scripts to increase the value
        // Reset this value to 50 after every turn
        globalActionPoints += increaseGAPBy;

        /*foreach (GameObject unit in playerUnits) {
            unit..GetComponent<Friendly>().firstAction = true;
        }*/
    }

    private IEnumerator StartBattle() {
        while (!currentTurn.Equals(GameState.BattleOver)) {
            turnNumber++;

            foreach (GameObject unit in playerUnits) {
                Lifeforms friendlyUnit = unit.GetComponent<Friendly>();
                friendlyUnit.StartRound();
            }

            Debug.Log("Battle Manager: Starting Player Turn");
            yield return StartCoroutine(GetComponent<PlayerTurn>().StartPlayerTurn());
            Debug.Log("Battle Manager: Ending Player Turn");
            //SwitchTurns();
            Debug.Log("Battle Manager: Starting Enemy Turn");
            //yield return StartCoroutine(GetComponent<EnemyTurn>().StartEnemyTurn(enemyUnits));
            Debug.Log("Battle Manager: Ending Enemy Turn");
            //SwitchTurns();
            yield return new WaitForSeconds(1);
        }
    }

    private void UnitDied(Lifeforms unit) {
        if (unit.GetComponent<Friendly>()) {
            for (int i = 0; i < playerUnits.Length; i++) {
                if (playerUnits[i] == unit.gameObject) {
                    deadFriendlyUnits.Add(unit.GetComponent<Friendly>());
                    playerUnits[i] = null;
                    break;
                }
            }
        } else if (unit.GetComponent<Enemy>()) {
            for (int i = 0; i < enemyUnits.Length; i++) {
                if (enemyUnits[i] == unit.gameObject) {
                    deadEnemyUnits.Add(unit.GetComponent<Enemy>());
                    enemyUnits[i] = null;
                    break;
                }
            }
        }

        UIManager.updateObjectiveText(unit);

        AllUnitsDied();
    }

    private void AllUnitsDied() {
        if (deadFriendlyUnits.Count == 4) {
            GameOver();
        } else if (deadEnemyUnits.Count == 4) {
            Victory();
        }
    }

    private void PlayerUnitDied() {

    }

    private void EnemyUnitDied() {

    }

    private void GameOver() {
        Debug.Log("Game Over");
        UIManager.Instance.ShowDefeatPanel();
        currentTurn = GameState.BattleOver;
    }

    private void Victory() {
        Debug.Log("Victory");
        UIManager.Instance.ShowVictoryPanel();
        SelectedLevel.CompleteLevel();
        currentTurn = GameState.BattleOver;
    }

    // Update is called once per frame
    void Update() {
        /*if (_currentBattleState == BattleState.PlayerAttack) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _currentBattleState = BattleState.PlayerIdle;
            }
        }*/
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

    public void ToggleButton(string btnToDisable, bool toggle) {
        switch (btnToDisable) {
            case "Stamina":
                // Probably better to have this in the action itself
                unitActionUI.transform.Find("StaminaStim").gameObject.GetComponent<Button>().interactable = toggle;
                break;
            default:
                break;
        }
    }

    public void ShowPlayerAttacks() {
        //battleActionsUI.SetActive(true);
    }

    public void PlayerRecovering(float value) {
        increaseGAPBy = value;
    }

    // Should make a ResetBattleUI method to reset UI to deafult state on PlayerTurn end
    //
    //

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
            unit.GetComponent<Light>().enabled = false;
        }
    }


    public void ClearPlayerTurn() {
        // Disable the lights on every player unit
        foreach (GameObject playerUnit in playerUnits) {
            playerUnit.GetComponent<Light>().enabled = false;
        }

        // Disable the lights on every enemy unit
        foreach (GameObject enemyUnit in enemyUnits) {
            enemyUnit.GetComponent<Light>().enabled = false;
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
