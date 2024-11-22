using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject unitStatsPanel;
    [SerializeField] private TMP_Text healthValTxt;
    [SerializeField] private TMP_Text damageValTxt;
    [SerializeField] private TMP_Text actionPointsValTxt;

    public static BattleManager Instance;

    public enum GameState {
        Intro,
        PlayerTurn,
        EnemyTurn
    }

    public GameState currentTurn { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentTurn = GameState.Intro;
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

        Debug.Log($"Current turn: {currentTurn}");
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
}
