using System.Collections;
using UnityEngine;

public class PlayerTurn : MonoBehaviour {
    [SerializeField] private Canvas BattleUI;

    private bool endTurn = false;

    private GameObject selectedUnit;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            selectedUnit = ClickManager.Instance.DetectClick();
            Debug.Log("Selected unit: " + selectedUnit.name);
        }
    }

    public IEnumerator StartPlayerTurn() {
        BattleUI.enabled = true;
        endTurn = false;
        Debug.Log("Player turn started");

        while (!endTurn) {
            yield return null;
        }

        Debug.Log("Player turn ended");
        BattleUI.enabled = false;
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
        selectedUnit.GetComponent<Lifeforms>().PerformMove("Basic");
    }

    public void SpecialMove() {
        Debug.Log("Executing special move");
        BattleManager.Instance.AttackingToggle();
        selectedUnit.GetComponent<Lifeforms>().PerformMove("Special");
    }

    public void UltimateMove() {
        Debug.Log("Executing ultimate move");
        BattleManager.Instance.AttackingToggle();
        selectedUnit.GetComponent<Lifeforms>().PerformMove("Ultimate");
    }
}
