using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("Selected Unit Header")]
    [SerializeField] private TMP_Text selectedUnitName;
    [SerializeField] private TMP_Text selectedUnitHealthTxt;
    [SerializeField] private TMP_Text selectedUnitAPTxt;
    [SerializeField] private TMP_Text selectedUnitAPRecoveryTxt;
    [SerializeField] private GameObject selectedUnitStatusEffects;

    [SerializeField] private GameObject turnUI;
    [SerializeField] private TMP_Text roundTxt;
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject actionsPanel;
    [SerializeField] private GameObject unitMoves;
    [SerializeField] private GameObject unitActions;

    [Header("Move Buttons")]
    [SerializeField] private Transform moveButtonsParent;
    [SerializeField] private GameObject moveBackButton;

    [Header("Action Buttons")]
    [SerializeField] private Transform actionButtonsParent;
    [SerializeField] private GameObject actionBackButton;

    [SerializeField] private Transform statusEffectParent;

    [Header("Confirmation UI")]
    [SerializeField] private GameObject confirmPage;
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private GameObject cancelBtn;
    [SerializeField] private TMP_Text confirmTxt;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TMP_Text costTxt;

    [Header("Temporary Variables")]
    [SerializeField] private TMP_Text objectiveTxt;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject pausePanel;


    [SerializeField] private float buttonSpacing = 100;

    public UnityEvent MoveComplete;

    public static Action<string> updateConfirmTxt;

    public static Action<Lifeforms> updateObjectiveText;

    public static UIManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //PlayerTurn.Instance.playerTurnEnded += EndPlayerTurn;
        updateConfirmTxt += UpdateConfirmTxt;
        updateObjectiveText += CheckDeadEnemies;
    }

    private void OnDestroy() {
        updateConfirmTxt -= UpdateConfirmTxt;
        Debug.Log("UIManager destroyed");
    }

    private void Start() {
        PlayerTurn.Instance.playerTurnStarted += StartPlayerTurn;
        PlayerTurn.Instance.playerTurnEnded += EndPlayerTurn;
    }

    public void UpdateStatBar(string name, float health, int actionPoints, int maxActionPoints, int actionPointRecovery) {
        selectedUnitName.SetText(name);
        selectedUnitHealthTxt.SetText($"Health: {health.ToString()}");
        selectedUnitAPTxt.SetText($"Action Points: {actionPoints.ToString()} / {maxActionPoints.ToString()}");
        selectedUnitAPRecoveryTxt.SetText($"AP Recovery: {actionPointRecovery.ToString()} AP");
    }

    public void UpdateStatBar(string name, float health) {
        selectedUnitName.SetText(name);
        selectedUnitHealthTxt.SetText($"Health: {health.ToString()}");
        selectedUnitAPTxt.SetText("");
    }

    public void ToggleStats(bool enable) {
        selectedUnitName.gameObject.SetActive(enable);
        selectedUnitHealthTxt.gameObject.SetActive(enable);
        selectedUnitAPTxt.gameObject.SetActive(enable);
        selectedUnitAPRecoveryTxt.gameObject.SetActive(enable);
        selectedUnitStatusEffects.SetActive(enable);
    }

    public void SwitchFriendlyUnit(Lifeforms unit) {
        LoadButtons(unit);
        ToggleStats(true);
        unitMoves.SetActive(false);
        unitActions.SetActive(false);
    }

    private void UpdateConfirmTxt(string value) {
        confirmTxt.SetText(value);
    }

    private void EndPlayerTurn() {
        ToggleStats(false);
        unitMoves.SetActive(false);
        unitActions.SetActive(false);
        actionsPanel.SetActive(false);
        turnUI.SetActive(false);
    }

    private void StartPlayerTurn() {
        battleUI.SetActive(true);
        turnUI.SetActive(true);
        roundTxt.SetText($"Round\n{BattleManager.Instance.turnNumber.ToString()}");
    }

    public void ShowVictoryPanel() {
        menuCanvas.SetActive(true);
        victoryPanel.SetActive(true);
    }

    public void ShowDefeatPanel() {
        menuCanvas.SetActive(true);
        defeatPanel.SetActive(true);
    }

    // Temporary. Change to a scripbable object
    private void CheckDeadEnemies(Lifeforms unit) {
        if (unit.CompareTag("Enemy")) {
            int deadEnemies = BattleManager.Instance.DeadEnemyUnits.Count;
            objectiveTxt.SetText($"Defeat all enemies\n{deadEnemies} / 4");
        }
    }

    public void LoadButtons(Lifeforms unit) {
        // Clear previous buttons
        foreach (Transform child in moveButtonsParent) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in actionButtonsParent) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in statusEffectParent) {
            Destroy(child.gameObject);
        }

        // Action buttons
        SetButtons(unit.GetActions(), unit, actionButtonsParent, actionBackButton);

        // Move buttons
        SetButtons(unit.GetMoves(), unit, moveButtonsParent, moveBackButton);

        SetEffects(unit);
    }

    private void SetButtons(ActionBase[] actions, Lifeforms unit, Transform parent, GameObject backButton) {
        GameObject newButton;

        foreach (var action in actions) {
            newButton = Instantiate(buttonPrefab, parent);
            confirmBtn.GetComponent<Button>().interactable = false;
            action.SetupButton(newButton.GetComponent<Button>(), unit, confirmPage, confirmBtn, cancelBtn.GetComponent<Button>());
        }

        newButton = Instantiate(backButton, parent);
        newButton.GetComponent<Button>().onClick.AddListener(() => parent.parent.gameObject.SetActive(false));

    }

    private void SetEffects(Lifeforms unit) {
        GameObject newEffect;
        if (unit.GetStatusEffects().Count == 0) return;
        foreach (var effect in unit.GetStatusEffects()) {
            newEffect = Instantiate(effect.effectPrefab, statusEffectParent);
        }
    }
}
