using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RepositionAction", menuName = "Actions/Reposition Action")]
public class RepositionAction : ActionBase {
private Vector3 positionToMoveTo;

    private GameObject confirmButton;
    private Button cancelButton;

    private GameObject placedMarker;

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        confirmButton = confirmBtn;
        cancelButton = cancelBtn;
        base.SetupButton(button, unit, confirmPage, confirmBtn, cancelBtn);

        //cancelBtn.interactable = false;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            confirmPage.SetActive(true);
            BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerMoving);
            ClickManager.Instance.FindMovePosition(SetMoveLocation, SetMarker);
            cancelBtn.onClick.RemoveAllListeners();
            OnClicked(confirmPage, confirmBtn, cancelBtn);
            Debug.Log("CLICKED BUTTON");
        });
    }

    protected override void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        //base.OnClicked(confirmPage, confirmBtn, cancelBtn);
        Debug.Log("ADDING LISTENERS");
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => {
            Debug.Log("Canceling move");
            CancelReposition(confirmPage);
        });

        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            //SetMovePosition();
            Debug.Log($"Moving to {positionToMoveTo}");
            PlayerTurn.Instance.StartReposition(this);

            confirmPage.SetActive(false);
            //Vector3 newMovePos = new Vector3(positionToMoveTo.x, 1, positionToMoveTo.z);
            //unit.transform.position = Vector3.MoveTowards(unit.transform.position, newMovePos, 1);
        });
    }

    // NEED TO SET CONFIRM BUTTON TO INTERACTABLE AFTER SETTING MOVE POSITION

    private void CancelReposition(GameObject confirmPage) {
        Debug.Log("Is this working?");
        Debug.Log($"PositionToMoveTo value: {positionToMoveTo}");
        if (positionToMoveTo == Vector3.zero) {
            confirmPage.SetActive(false);
            BattleManager.Instance.changeBattleState?.Invoke(BattleManager.BattleState.PlayerIdle);
            ClickManager.Instance.CancelFollowMouse();
        } else {
            positionToMoveTo = Vector3.zero;
            Debug.Log($"POSITION TO MOVE TO: {positionToMoveTo}");
            confirmButton.GetComponent<Button>().interactable = false;
            ClickManager.Instance.CancelFollowMouse();
            Destroy(placedMarker.gameObject);
            ClickManager.Instance.FindMovePosition(SetMoveLocation, SetMarker);
        }
    }

    private void SetMoveLocation(Vector3 position) {
        positionToMoveTo = position;
        confirmButton.GetComponent<Button>().interactable = true;
        Debug.Log(position);
    }

    private void SetMarker(GameObject marker) {
        placedMarker = marker;
    }

    public void SetMovePosition() {
        BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerMoving);
    }

    public override IEnumerator Execute(Lifeforms unit) {
        Debug.Log("Repositioning unit");
        //return base.Execute(unit);
        Vector3 newMovePos = positionToMoveTo;
        

        while (unit.transform.position != newMovePos) {
            unit.gameObject.transform.position = Vector3.MoveTowards(unit.gameObject.transform.position, newMovePos, Time.deltaTime * 5);
            yield return null;
        }

        Destroy(placedMarker.gameObject);

        yield break;
    }
}
