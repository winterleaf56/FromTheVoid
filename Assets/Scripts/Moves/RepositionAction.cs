using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RepositionAction", menuName = "Actions/Reposition Action")]
public class RepositionAction : ActionBase {

    private Vector3 positionToMoveTo;

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.SetupButton(button, unit, confirmPage, confirmBtn, cancelBtn);

        button.onClick.AddListener(() => {
            ClickManager.Instance.FindMovePosition(SetMoveLocation);
        });

        /*confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            //SetMovePosition();
            Debug.Log($"Moving to {positionToMoveTo}");
            Vector3 newMovePos = new Vector3(positionToMoveTo.x, 1, positionToMoveTo.z);
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, newMovePos, 1);
        });*/
        
    }

    // NEED TO SET CONFIRM BUTTON TO INTERACTABLE AFTER SETTING MOVE POSITION

    private void SetMoveLocation(Vector3 position) {
        positionToMoveTo = position;
        Debug.Log(position);
    }

    public void SetMovePosition() {
        BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerMoving);
    }

    public IEnumerator Execute(Lifeforms unit, Vector3 movePosition) {
        return base.Execute(unit);
    }
}
