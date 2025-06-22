using System;
using System.Collections.Generic;
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

    [SerializeField] private GameObject pauseCanvas;

    [SerializeField] private GameObject endTurnBtn;

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
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private TMP_Text objectivePrefab;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private float buttonSpacing = 100;

    private Dictionary<ObjectiveBase, TMP_Text> objectiveTexts = new Dictionary<ObjectiveBase, TMP_Text>();

    public UnityEvent MoveComplete;

    public static Action<string> updateConfirmTxt;
    public static Action<bool> ButtonsToggle;
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
        ButtonsToggle += ToggleButtons;

        
    }

    private void OnDestroy() {
        updateConfirmTxt -= UpdateConfirmTxt;
        updateObjectiveText -= CheckDeadEnemies;
        ButtonsToggle -= ToggleButtons;
        Debug.Log("UIManager destroyed");
    }

    private void Start() {
        PlayerTurn.Instance.playerTurnStarted += StartPlayerTurn;
        PlayerTurn.Instance.playerTurnEnded += EndPlayerTurn;
        BattleManager.onGamePaused += PauseGame;

        SetObjectives();
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

    private void PauseGame(bool value) {
        menuCanvas.SetActive(value);
        menuCanvas.transform.Find("PausePanel").gameObject.SetActive(value);
    }

    private void ToggleButtons(bool value) {
        if (value) {
            endTurnBtn.SetActive(true);
            actionsPanel.SetActive(true);
        } else {
            endTurnBtn.SetActive(false);
            actionsPanel.SetActive(false);
        }
    }

    private void SetObjectives() {
        // Clear existing objective UI elements except the title text
        foreach (Transform child in objectivePanel.transform) {
            if (child.name == "ObjectivePrefab")
                Destroy(child.gameObject);
        }

        objectiveTexts.Clear();

        // Add each objective to the UI
        foreach (var objective in BattleManager.SelectedLevel.Objectives) {
            TMP_Text newObjective = Instantiate(objectivePrefab, objectivePanel.transform);
            newObjective.SetText(objective.objectiveText);

            // Add the objective to the dictionary
            // Remember, objectiveTexts[objective] is the key (using the ObjectiveBase reference) and newObjective is the value (the TMP_Text component)
            objectiveTexts[objective] = newObjective;
        }
    }

    public void UpdateObjectiveUI(ObjectiveBase objective) {
        if (objectiveTexts.ContainsKey(objective)) {
            TMP_Text objectiveText = objectiveTexts[objective];

            // Change color to green if completed
            if (objective.isCompleted) {
                objectiveText.color = Color.green;
            }
        }
    }

    // Temporary. Change to a scripbable object
    private void CheckDeadEnemies(Lifeforms unit) {
        if (unit.CompareTag("Enemy")) {
            int deadEnemies = BattleManager.Instance.DeadEnemyUnits.Count;
            //objectiveTxt.SetText($"Defeat all enemies\n{deadEnemies} / 4");
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
