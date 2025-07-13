using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RecoverAction", menuName = "Actions/General/Recover Action")]
public class RecoverAction : ActionBase {
    // This action will recover the player's Action Points by a set amount next turn
    // Can only be used if player has not made a move, they can reposition a short distance though.
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Button button;
    [SerializeField] private int recoveryAmount = 50;
    [SerializeField] private int duration = 2;

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.SetupButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        //button.GetComponentInChildren<TMP_Text>().SetText("Recover AP");

        foreach (var effect in unit.GetStatusEffects()) {
            if (effect.GetData().EffectName == "Recovering") {
                DisableButton(button);
                break;
            }
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            confirmPage.SetActive(true);
            ClickManager.Instance.allowClicks = false;
            confirmBtn.GetComponent<Button>().interactable = true;
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerAction);
            OnClicked(confirmPage, confirmBtn, cancelBtn);
            
        });
    }

    protected override void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        //base.OnClicked(confirmPage, confirmBtn, cancelBtn);

        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => {
            confirmPage.SetActive(false);
            ClickManager.Instance.allowClicks = true;
        });

        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            PlayerTurn.Instance.RecoverAction();
            confirmPage.SetActive(false);
        });
    }

    public override IEnumerator Execute(Lifeforms unit) {
        unit.stats.SubtractActionPoints(actionPointCost);
        
        StatusEffectManager.Instance.AddEffect(StatusEffectManager.StatusEffectType.ActionPointEffect, effectPrefab, "Recovering", duration, recoveryAmount, unit);

        OnMoveFinished();
        ClickManager.Instance.allowClicks = true;
        yield break;
    }

    /*protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        button.GetComponentInChildren<TMP_Text>().SetText("Recover AP");
    }*/
}
