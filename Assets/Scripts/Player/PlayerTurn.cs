using System.Collections;
using TMPro;
using UnityEngine;
using static BattleManager;

public class PlayerTurn : MonoBehaviour {
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject actionsUI;

    [SerializeField] private TMP_Text selectedUnitName;
    [SerializeField] private TMP_Text selectedUnitHealthTxt;
    [SerializeField] private TMP_Text selectedUnitAPTxt;

    private bool endTurn = false;

    private Lifeforms selectedEnemy;
    private Lifeforms selectedFriendly;

    // Reselecting an enemy will not disable the UI so need to fix this
    // I am not sure what I meant by this as everything seems to work
    // Update: I meant ActionsUI will not disable on clicking an enemy (This goes in hand with change in PlayerTurn [If Friendly selected, click anywhere to unselect and turn off light])

    private GameObject _selectedUnit;
    public GameObject selectedUnit {
        get => _selectedUnit;
        private set {
            _selectedUnit = value;

            if (_selectedUnit.CompareTag("Friendly")) {
                print("Friendly unit selected, activating actionsUI");
                actionsUI.SetActive(true);
                selectedFriendly = _selectedUnit.GetComponent<Lifeforms>();
                ToggleStats(true);
                selectedUnitName.SetText(selectedFriendly.stats.Name);
                selectedUnitHealthTxt.SetText($"Health: {selectedFriendly.stats.MaxHealth.ToString()}");
                selectedUnitAPTxt.SetText($"Action Points: {selectedFriendly.stats.ActionPoints.ToString()}/{selectedFriendly.maxActionPoints}");
            } else if (!BattleManager.Instance.currentBattleState.Equals(BattleState.PlayerAttack)) {
                print("Enemy unit selected, deactivating actionsUI");
                actionsUI.SetActive(false);
            } else if (_selectedUnit.CompareTag("UI")) {
                return;
            } else {
                selectedFriendly = null;
                selectedEnemy = null;
                ToggleStats(false);
            }

            Debug.Log("SELECTED UNIT CHANGED");

            if (BattleManager.Instance.currentBattleState.Equals(BattleState.PlayerAttack) && _selectedUnit.CompareTag("Enemy")) {
                //Debug.Log($"Enemy unit recieved and being set to selectedEnemy (TAG): {selectedUnit.tag}");
                selectedEnemy = _selectedUnit.GetComponent<Lifeforms>();
            }
            //_selectedUnit = value;

            /*if (_selectedUnit.CompareTag("Enemy")) {
                Debug.Log("Enemy unit selected");
                selectedEnemy = _selectedUnit.GetComponent<Lifeforms>();
            }*/
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log($"Selected Unit through Update PlayerTurn: {_selectedUnit}");
            selectedUnit = ClickManager.Instance.DetectClick();
            //Debug.Log($"Selected Unit through Update PlayerTurn: {_selectedUnit}");
            Debug.Log($"Currently selected Friendly unit: {selectedFriendly}");
            Debug.Log($"Currently selected enemy: {selectedEnemy}");
            //Debug.Log("Selected unit: " + selectedUnit.GetComponent<Lifeforms>().stats.Name);
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

    private void CancelAttack() {
        Debug.Log("Attack cancelled");
        BattleManager.Instance.AttackingToggle();
    }

    /*public void ConfirmAttack() {
        Debug.Log("Attack confirmed");
        BattleManager.Instance.AttackingToggle();
        BasicMove();
    }*/

    public void EndTurn() {
        endTurn = true;
    }

    // If player has enough AP, button clickable, Not enough AP, unclickable, greyed out

    public bool DeductAP(string move) {
        int apRequirement = selectedFriendly.GetMoveAPRequirement(move);
        if (selectedFriendly.getActionPoints() >= apRequirement) {
            return true;
        } else {
           return false;
        }
    }

    public void BasicMove() {
        if (!DeductAP("Basic")) {
            Debug.Log("Not enough AP to perform basic move");
            return;
        }
        Debug.Log("Executing basic move");
        BattleManager.Instance.AttackingToggle();
        selectedFriendly.PerformMove("Basic", selectedEnemy);
        Debug.Log($"Selected unit using basic move: {selectedUnit.GetComponent<Lifeforms>().stats.Name}");
        Debug.Log($"Selected enemy recieving basic move: {selectedEnemy.stats.Name}");
        //BattleManager.Instance.ClearPlayerTurn();
    }

    void ToggleStats(bool enable) {
        /*suName.SetText(selectedFriendly.stats.Name);
        suHealthTxt.SetText(selectedFriendly.stats.MaxHealth.ToString());
        suAPTxt.SetText(selectedFriendly.stats.ActionPoints.ToString());*/

        selectedUnitName.gameObject.SetActive(enable);
        selectedUnitHealthTxt.gameObject.SetActive(enable);
        selectedUnitAPTxt.gameObject.SetActive(enable);

        /*if (enable) {
            suName.enabled = true;
            suHealthTxt.enabled = true;
            suAPTxt.enabled = true;
        } else {
            suName.enabled = false;
            suHealthTxt.enabled = false;
            suAPTxt.enabled = false;
        }*/
    }

    public void SpecialMove() {
        Debug.Log("Executing special move");
        BattleManager.Instance.AttackingToggle();
        selectedFriendly.PerformMove("Special", selectedEnemy);
        //BattleManager.Instance.ClearPlayerTurn();
    }

    public void UltimateMove() {
        Debug.Log("Executing ultimate move");
        BattleManager.Instance.AttackingToggle();
        selectedFriendly.PerformMove("Ultimate", selectedEnemy);
        //BattleManager.Instance.ClearPlayerTurn();
    }
}
