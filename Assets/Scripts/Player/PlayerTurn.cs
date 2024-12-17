using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour {
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject actionsUI;

    [SerializeField] private Button confirmBtn;

    private UIManager UIManager => UIManager.Instance;

    private bool endTurn = false;

    public List<Enemy> AttackableEnemies { get; private set; }

    private Lifeforms selectedEnemy;
    private Lifeforms selectedFriendly;
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
                UIManager.LoadButtons(selectedFriendly);
                UIManager.ToggleStats(true);
                UIManager.UpdateStatBar(selectedFriendly.stats.UnitName, selectedFriendly.stats.MaxHealth, selectedFriendly.stats.ActionPoints);
            } else if (!BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack)) {
                print("Enemy unit selected, deactivating actionsUI");
                actionsUI.SetActive(false);
                selectedFriendly.GetComponent<Light>().enabled = false;
                UIManager.ToggleStats(false);
            } else if (_selectedUnit.CompareTag("UI")) {
                return;
            } else {
                if (selectedFriendly == null) {
                    //selectedFriendly = null;
                }
                /*selectedFriendly = null;
                selectedEnemy = null;*/
                //ToggleStats(false);
            }

            Debug.Log("SELECTED UNIT CHANGED");

            if (BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack) && _selectedUnit.CompareTag("Enemy") && AttackableEnemies.Contains(_selectedUnit.GetComponent<Enemy>())) {
                selectedEnemy = _selectedUnit.GetComponent<Lifeforms>();
                confirmBtn.interactable = true;
            }
            //_selectedUnit = value;

            /*if (_selectedUnit.CompareTag("Enemy")) {
                Debug.Log("Enemy unit selected");
                selectedEnemy = _selectedUnit.GetComponent<Lifeforms>();
            }*/
        }
    }

    public static PlayerTurn Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    public IEnumerator StartPlayerTurn() {
        BattleUI.SetActive(true);
        endTurn = false;
        Debug.Log("Player turn started");



        while (!endTurn) {
            yield return null;
        }

        Debug.Log("Player turn ended");
        BattleUI.SetActive(false);
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

    public void BasicMove() {
        if (!CheckAP("Basic")) {
            Debug.Log("Not enough AP to perform basic move");
            return;
        } else {
            Debug.Log("TEST THJINGSAD");
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
