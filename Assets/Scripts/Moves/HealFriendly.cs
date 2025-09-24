using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Action", menuName = "Actions/Medic/Heal Friendly")]
public class HealFriendly : FriendlyTargetMove {

    // This class is a basic healing action. Click to heal the selected friendly unit.
    // More advanced heals can be created by inheriting from this class.
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Button button;
    [SerializeField] private int recoveryAmount = 50;
    [SerializeField] private int duration = 2;

    public virtual float CalculateHeal() {
        return value;
    }

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            //BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerAction);
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerFriendlyAction);
            OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn, rangeRing);
            //OnClicked(confirmPage, confirmBtn, cancelBtn);
            Debug.Log("Setting up button for Healing Friendly Status Effect");
        });

    }

    /*protected override void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        //base.OnClicked(confirmPage, confirmBtn, cancelBtn);

        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => {
            confirmPage.SetActive(false);
            //BattleManager.manageFriendlyLights?.Invoke(GetFriendliesInRange(unit));
            ClickManager.Instance.allowClicks = true;
        });

        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            //PlayerTurn.Instance.RecoverAction();
            PlayerTurn.Instance.StartDirectedFriendlyAction(this);
            confirmPage.SetActive(false);
        });
    }*/

    protected virtual void OnClickedBasic(Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn, GameObject rangeRing) {
        base.OnClicked(confirmPage, confirmBtn, cancelBtn);

        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            //PlayerTurn.Instance.StartDirectAttack(this);
            PlayerTurn.Instance.StartDirectedFriendlyAction(this);
            Debug.Log("Confirming heal friendly action");
            confirmPage.gameObject.SetActive(false);
            rangeRing.SetActive(false);
        });

        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => {
            BattleManager.manageFriendlyLights?.Invoke(GetFriendliesInRange(unit));
            BattleManager.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
            PlayerTurn.Instance.CancelMove(unit);
            rangeRing.SetActive(false);
            confirmPage.gameObject.SetActive(false);
        });

        /*cancelBtn.GetComponent<Button>().onClick.AddListener(() => {
            //BattleManager.Instance.ManageLights(enemyList);
            Debug.Log("Canceling move");
            confirmPage.gameObject.SetActive(false);
        });*/
    }

    public override IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        unit.stats.SubtractActionPoints(actionPointCost);

        //StatusEffectManager.Instance.AddEffect(StatusEffectManager.StatusEffectType.ActionPointEffect, effectPrefab, "Recovering", duration, recoveryAmount, unit);
        StatusEffectManager.Instance.AddEffect(StatusEffectManager.StatusEffectType.HealingEffect, effectPrefab, "Healing", duration, recoveryAmount, target);

        OnMoveFinished(unit);
        ClickManager.Instance.allowClicks = true;
        yield break;
    }
}
