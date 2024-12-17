using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    [Header("Display Selected Unit Stats")]
    [SerializeField] private GameObject unitStatsPanel;
    [SerializeField] private TMP_Text healthValTxt;
    [SerializeField] private TMP_Text damageValTxt;
    [SerializeField] private TMP_Text actionPointsValTxt;

    [Header("UI Elements")]
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject battleActionsUI;
    [SerializeField] private GameObject unitMovesUI;
    [SerializeField] private GameObject unitActionUI;
    [SerializeField] private GameObject battleAttackBtnUI;
    [SerializeField] private GameObject unitStatUI;

    [Header("Lighting")]
    [SerializeField] private GameObject selectingEnemyLight;
    [SerializeField] private GameObject worldLight;

    [Header("Unit References")]
    [SerializeField] private GameObject friendlyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] friendlySpawns;
    [SerializeField] private GameObject[] enemySpawns;

    public GameObject[] playerUnits;
    public GameObject[] enemyUnits;

    public int turnNumber { get; private set; }
    public int waveNumber { get; private set; } // Waves not implemented, add at later time

    public float globalActionPoints { get; private set; }
    public float increaseGAPBy { get; private set; } = 50;

    public Action onPlayerTurnStart;
    public Action<BattleState> changeBattleState;
    public Action onMoveFinished;

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

        currentTurn = GameState.Intro;

        enemyUnits = new GameObject[enemySpawns.Length];

        for (int i = 0; i < friendlySpawns.Length; i++) {
            playerUnits[i] = Instantiate(playerUnits[i], friendlySpawns[i].transform.position, Quaternion.identity);
        }

        for (int i = 0; i < enemySpawns.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawns[i].transform.position, Quaternion.identity);
            enemyUnits[i] = enemy;

        }

        currentTurn = GameState.PlayerTurn;
    }

    [SerializeField] Canvas randomButtonCanvas;

    void Start() {
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
            Debug.Log("Battle Manager: Starting Player Turn");
            yield return StartCoroutine(GetComponent<PlayerTurn>().StartPlayerTurn());
            Debug.Log("Battle Manager: Ending Player Turn");
            //SwitchTurns();
            Debug.Log("Battle Manager: Starting Enemy Turn");

            Debug.Log("Battle Manager: Ending Enemy Turn");
            //SwitchTurns();
            yield return new WaitForSeconds(1);
        }
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

    public void AttackingToggle() {
        //currentTurn = (currentTurn == GameState.PlayerTurn) ? GameState.EnemyTurn : GameState.PlayerTurn;
        currentBattleState = (currentBattleState == BattleState.PlayerAttack) ? BattleState.PlayerIdle : BattleState.PlayerAttack;
        Debug.Log($"Changing state to: {currentBattleState}");

        /*foreach (GameObject enemy in enemyUnits) {
            Light displayLight = enemy.GetComponent<Light>();
            displayLight.color = Color.green;
            displayLight.enabled = !displayLight.enabled;
        }*/

        // Change Lighting
        //ToggleWorldLight();
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

        //battleUI.SetActive(false);
        battleActionsUI.SetActive(false);
        foreach (Transform child in battleAttackBtnUI.transform) {
            if (child.name != "BackBtn") {
                child.gameObject.SetActive(false);
            }
        }
    }
}
