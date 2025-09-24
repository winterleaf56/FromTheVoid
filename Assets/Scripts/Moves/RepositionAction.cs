using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RepositionAction", menuName = "Actions/General/Reposition Action")]
public class RepositionAction : ActionBase {
    private Vector3 positionToMoveTo;

    private GameObject confirmButton;
    private Button cancelButton;

    private GameObject placedMarker;
    private int unitAP;
    private float moveCost;
    private Lifeforms unitToMove;

    public int moveCostMultiplier = 4;

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        confirmButton = confirmBtn;
        cancelButton = cancelBtn;
        base.SetupButton(button, unit, confirmPage, confirmBtn, cancelBtn);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            unitAP = unit.stats.ActionPoints;
            confirmPage.SetActive(true);
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerMoving);
            unitToMove = unit;
            ClickManager.Instance.FindMovePosition(unitToMove, unitAP, actionPointCost, SetValues);
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
            UIManager.updateConfirmTxt("");

            confirmPage.SetActive(false);
        });
    }

    private void CancelReposition(GameObject confirmPage) {
        Debug.Log($"PositionToMoveTo value: {positionToMoveTo}");
        if (positionToMoveTo == Vector3.zero) {
            confirmPage.SetActive(false);
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerIdle);
            ClickManager.Instance.CancelFollowMouse();
        } else {
            positionToMoveTo = Vector3.zero;
            Debug.Log($"POSITION TO MOVE TO: {positionToMoveTo}");
            confirmButton.GetComponent<Button>().interactable = false;
            ClickManager.Instance.CancelFollowMouse();
            if (placedMarker != null) Destroy(placedMarker.gameObject);
            //ClickManager.Instance.FindMovePosition(unitAP, SetMoveLocation, SetMarker, SetCost);
            ClickManager.Instance.FindMovePosition(unitToMove, unitAP, actionPointCost, SetValues);
        }
    }

    private void SetValues(Vector3 position, GameObject marker, float cost) {
        positionToMoveTo = position;
        confirmButton.GetComponent<Button>().interactable = true;

        placedMarker = marker;

        NavMeshAgent agent = unitToMove.GetComponent<NavMeshAgent>();
        if (agent != null) {
            float pathDistance = NavigationUtils.CalculatePathDistance(agent, position);

            if (pathDistance < Mathf.Infinity) {
                moveCost = pathDistance * actionPointCost;
            } else {
                Debug.LogWarning("Invalid path to the target position.");
                moveCost = Mathf.Infinity;
            }
        } else {
            Debug.LogError("NavMeshAgent not found on the unit.");
            moveCost = Mathf.Infinity;
        }

        moveCost = Mathf.Round(moveCost);

        string stringToSend = $"Movement Cost:\n{moveCost.ToString()} AP";
        UIManager.updateConfirmTxt(stringToSend);
        Debug.Log($"Position: {positionToMoveTo}, Marker: {placedMarker}, Cost: {moveCost}");
    }

    public override IEnumerator Execute(Lifeforms unit) {
        Debug.Log("Repositioning unit");
        //return base.Execute(unit);
        Vector3 newMovePos = positionToMoveTo;
        
        unit.stats.SubtractActionPoints((int)moveCost);

        BattleManager.audioClip?.Invoke(actionSound, unit.transform.position);

        Navigation navigation = unit.GetComponent<Navigation>();
        if (navigation == null) {
            Debug.LogError("No navigation component found");
            yield break;
        }

        bool movementComplete = false;
        navigation.MoveTo(positionToMoveTo, () => {
            movementComplete = true;
            Debug.Log("Movement complete");
        });

        while (!movementComplete) {
            yield return null;
        }

        /*while (unit.transform.position != newMovePos) {
            unit.gameObject.transform.position = Vector3.MoveTowards(unit.gameObject.transform.position, newMovePos, Time.deltaTime * 5);
            yield return null;
        }*/

        if (placedMarker != null) Destroy(placedMarker.gameObject);

        OnMoveFinished(unit);
        positionToMoveTo = Vector3.zero;

        yield break;
    }
}
