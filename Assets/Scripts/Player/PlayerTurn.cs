using System.Collections;
using UnityEngine;
using static BattleManager;

public class PlayerTurn : MonoBehaviour {
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject actionsUI;

    private bool endTurn = false;

    private Lifeforms selectedEnemy;
    private Lifeforms selectedFriendly;

    private GameObject _selectedUnit;
    public GameObject selectedUnit {
        get => _selectedUnit;
        private set {
            _selectedUnit = value;

            if (_selectedUnit.CompareTag("Friendly")) {
                print("Friendly unit selected, activating actionsUI");
                actionsUI.SetActive(true);
                selectedFriendly = _selectedUnit.GetComponent<Lifeforms>();
            } else if (!BattleManager.Instance.currentBattleState.Equals(BattleState.PlayerAttack)) {
                print("Enemy unit selected, deactivating actionsUI");
                actionsUI.SetActive(false);
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

    public void BasicMove() {
        Debug.Log("Executing basic move");
        BattleManager.Instance.AttackingToggle();
        selectedFriendly.PerformMove("Basic", selectedEnemy);
        Debug.Log($"Selected unit using basic move: {selectedUnit.GetComponent<Lifeforms>().stats.Name}");
        Debug.Log($"Selected enemy recieving basic move: {selectedEnemy.stats.Name}");
        //BattleManager.Instance.ClearPlayerTurn();
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
