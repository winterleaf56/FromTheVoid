using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour {
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject turnUI;
    [SerializeField] private GameObject actionsUI;

    [SerializeField] private Button confirmBtn;

    private UIManager UIManager => UIManager.Instance;

    private bool endTurn = false;

    public Action changedAP;
    public Action playerTurnStarted;
    public Action playerTurnEnded;

    public List<Enemy> AttackableEnemies { get; private set; }
    public List<Friendly> TargetableFriendlies { get; private set; }

    private Lifeforms selectedEnemy;
    public Lifeforms selectedFriendly { get; private set; }
    public Lifeforms targetedFriendly { get; private set; }
    private Lifeforms lastSelectedEnemy;
    private Lifeforms lastSelectedFriendly;

    private GameObject _selectedUnit;
    public GameObject selectedUnit {
        get => _selectedUnit;
        private set {
            _selectedUnit = value;
            print($"Selected unit: {_selectedUnit}");

            if (_selectedUnit.CompareTag("Friendly")) {
                print("Friendly unit selected, activating actionsUI");
                actionsUI.SetActive(true);
                selectedFriendly = _selectedUnit.GetComponent<Lifeforms>();
                selectedFriendly.GetComponent<Light>().enabled = true;
                //UIManager.LoadButtons(selectedFriendly);
                GetButtonInfo();
                //UIManager.ToggleStats(true);
                Stats stats = selectedFriendly.stats;
                UIManager.UpdateStatBar(stats.UnitName, selectedFriendly.health.GetHealth(), stats.ActionPoints, stats.MaxActionPoints, stats.ActionPointRecovery);
            } else if (!BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack)) {
                print("Enemy unit selected, deactivating actionsUI");
                actionsUI.SetActive(false);
                selectedFriendly.GetComponent<Light>().enabled = false;
                UIManager.ToggleStats(false);
            } else if (_selectedUnit.CompareTag("UI")) {
                return;
            }

            Debug.Log("SELECTED UNIT CHANGED");

            if (BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack) && _selectedUnit.CompareTag("Enemy") && AttackableEnemies.Contains(_selectedUnit.GetComponent<Enemy>())) {
                selectedEnemy = _selectedUnit.GetComponent<Lifeforms>();
                confirmBtn.interactable = true;
            }
        }
    }

    public static PlayerTurn Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        changedAP += MoveFinished;
    }

    /*private void ManageLights() {
        selectedFriend
    }*/

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            GameObject temp = ClickManager.Instance.DetectClick();
            if (temp != null) {
                selectedUnit = temp;
            }
        }
    }

    public void GetButtonInfo() {
        UIManager.SwitchFriendlyUnit(selectedFriendly);
    }

    /*public void GetButtonInfo() {
        UIManager.LoadButtons(selectedFriendly);
        UIManager.ToggleStats(true);
    }*/

    private void MoveFinished() {
        Stats stats = selectedFriendly.stats;
        UIManager.UpdateStatBar(stats.UnitName, stats.MaxHealth, stats.ActionPoints, stats.MaxActionPoints, stats.ActionPointRecovery);
        GetButtonInfo();
    }

    public IEnumerator StartPlayerTurn() {
        playerTurnStarted?.Invoke();
        turnUI.SetActive(true);
        endTurn = false;
        ClickManager.Instance.allowClicks = true;
        StatusEffectManager.Instance.UpdateFriendlyEffects();
        Debug.Log("Player turn started");



        while (!endTurn) {
            yield return null;
        }

        //playerTurnEnded?.Invoke();
        Debug.Log("Player turn ended");
        turnUI.SetActive(false);
    }

    private void ResetSelectedUnits() {
        selectedFriendly = null;
        selectedEnemy = null;
    }

    /*public void ConfirmAttack() {
        Debug.Log("Attack confirmed");
        BattleManager.Instance.AttackingToggle();
        BasicMove();
    }*/

    public void EndTurn() {
        endTurn = true;
        playerTurnEnded?.Invoke();
    }

    public void CancelMove() {
        selectedEnemy = null;
    }

    void LoadMoveButtons() {
        Debug.Log($"Getting moves from selected unit: {selectedFriendly.stats.UnitName}");

        ActionBase[] actions = selectedFriendly.GetComponents<ActionBase>();
        foreach (ActionBase action in actions) {
            Debug.Log($"Action: {action.name}");
        }
    }

    public void SetAttackableEnemies(List<Enemy> enemyList) {
        AttackableEnemies = enemyList;
    }

    public void SetTargetableFriendlies(List<Friendly> friendlyList) {
        TargetableFriendlies = friendlyList;
    }


    // If player has enough AP, button clickable, Not enough AP, unclickable, greyed out

    public bool CheckAP(string move) {
        Debug.Log($"Checking AP for {move} move");
        int apRequirement = selectedFriendly.GetMoveAPRequirement(move);
        Debug.Log($"AP Requirement for {move} move: {apRequirement}");
        Debug.Log($"Current AP: {selectedFriendly.stats.ActionPoints}");
        if (selectedFriendly.stats.ActionPoints >= apRequirement) {
            return true;
        } else {
            return false;
        }
    }

    public void StartDirectAttack(ActionBase move) {
        Debug.Log($"Starting direct attack from StartDirectAttack: {move.name}");
        selectedFriendly.PerformDirectAttack(move, selectedEnemy);
    }

    public void StartAction(ActionBase action) {
        Debug.Log($"Starting action from StartAction: {action.name}");
        selectedFriendly.PerformAction(action);
    }

    public void StartReposition(RepositionAction reposition) {
        Debug.Log($"Starting reposition from StartReposition: {reposition.name}");
        selectedFriendly.PerformReposition(reposition);
    }

    // New addition in 0.1.4.1
    // This method is used to start a directed action on a friendly unit (Healing, Buffing, etc.)
    // Now realizing I need a second selectedFriendly variable for this to work properly
    // Best coursse of action is to have click manager detect BattleState.PlayerFriendlyAction
    public void StartDirectedFriendlyAction(ActionBase action) {
        selectedFriendly.PerformFriendlyDirectedAction(action, selectedFriendly);
    }

    // Everything below this point can probably be removed as it is replaced by the above code

    public void BasicMove() {
        if (!CheckAP("Basic")) {
            Debug.Log("Not enough AP to perform basic move");
            return;
        }
        Debug.Log("Executing basic move");
        //BattleManager.Instance.AttackingToggle();
       // BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
        selectedFriendly.PerformMove("Basic", selectedEnemy);
        Debug.Log($"Selected unit using basic move: {selectedUnit.GetComponent<Lifeforms>().stats.UnitName}");
        Debug.Log($"Selected enemy recieving basic move: {selectedEnemy.stats.UnitName}");
        //BattleManager.Instance.ClearPlayerTurn();
        ResetSelectedUnits();
    }

    public void SpecialMove() {
        Debug.Log("Executing special move");
        //BattleManager.Instance.AttackingToggle();
        //BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
        selectedFriendly.PerformMove("Special", selectedEnemy);
        //BattleManager.Instance.ClearPlayerTurn();
        ResetSelectedUnits();
    }

    public void UltimateMove() {
        Debug.Log("Executing ultimate move");
        //BattleManager.Instance.AttackingToggle();
        //BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
        selectedFriendly.PerformMove("Ultimate", selectedEnemy);
        //BattleManager.Instance.ClearPlayerTurn();
        ResetSelectedUnits();
    }

    public void RecoverAction() {
        Debug.Log("Executing recover action");
        //BattleManager.Instance.AttackingToggle();
        selectedFriendly.PerformAction("Recover", selectedFriendly);
        //BattleManager.Instance.ClearPlayerTurn();
        ResetSelectedUnits();
    }

    public void StaminaStimAction() {
        Debug.Log("Executing Stamina Stim Action");
        selectedFriendly.GetComponentInParent<Assault>().PerformAction("Stamina", selectedFriendly);
        ActionComplete();
    }

    private void ActionComplete() {
        ResetSelectedUnits();
        // Reset UI in UIManager when move is complete
    }
}
