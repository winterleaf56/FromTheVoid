using System;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject unitStatsPanel;
    [SerializeField] private TMP_Text healthValTxt;
    [SerializeField] private TMP_Text damageValTxt;
    [SerializeField] private TMP_Text actionPointsValTxt;

    [SerializeField] private GameObject battleActionsUI;

    [SerializeField] private GameObject friendlyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] friendlySpawns;
    [SerializeField] private GameObject[] enemySpawns;

    public GameObject[] playerUnits;
    public GameObject[] enemyUnits;

    public float globalActionPoints { get; private set; }
    public float increaseGAPBy { get; private set; } = 50;

    public Action onPlayerTurnStart;

    public static BattleManager Instance;

    public enum GameState {
        Intro,
        PlayerTurn,
        EnemyTurn
    }

    public enum BattleState {
        PlayerIdle,
        PlayerAttack,
        PlayerDefend,
        PlayerItem
    }

    public GameState currentTurn { get; private set; }
    public BattleState currentBattleState { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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

    private void startPlayerTurn() {
        // Increases the global action points by the value of increaseGAPBy which can be changed in other scripts to increase the value
        // Reset this value to 50 after every turn
        globalActionPoints += increaseGAPBy;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void ShowPlayerAttacks() {
        battleActionsUI.SetActive(true);
    }

    public void PlayerRecovering(float value) {
        increaseGAPBy = value;
    }
}
