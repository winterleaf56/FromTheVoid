using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Basic Attack")]
public class BasicMove : ActionBase {

    public virtual float CalculateDamage() {
        return damage;
    }

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerAttack);
            OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn);
            Debug.Log("Executing basic move by setting up button");
        });

    }

    protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }

    // OnClickedBasic is OnClicked but toggles BattleState to PlayerAttack so children can override OnClicked and not have to worry about toggling BattleState to PlayerAttack
    protected virtual void OnClickedBasic(Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.OnClicked(confirmPage, confirmBtn, cancelBtn);

        confirmBtn.GetComponent<Button>().onClick.AddListener(() => {
            PlayerTurn.Instance.StartDirectAttack(this);
        });

        cancelBtn.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            BattleManager.Instance.ManageLights(GetEnemiesInRange(unit));
            BattleManager.Instance.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
            //PlayerTurn.Instance.StartDirectAttack(this);
            PlayerTurn.Instance.CancelMove();
        });
    }

    // Make a coroutine that runs while the player is selecting an enemy to attack, then when the attack occurs, the coroutine ends
    public override IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log($"Unit: {unit}, Target: {target}");
        OnMoveFinished(unit);
        ClickManager.Instance.allowClicks = false;
        Debug.Log($"Performing move: {moveName}, against: {target}");

        // Damage the target
        target.GetComponentInParent<Health>().TakeDamage(CalculateDamage());
        Debug.Log($"Damaging {target} for {CalculateDamage()} damage.");

        // Subtract action points for executing move
        unit.stats.SubtractActionPoints(actionPointCost);

        int i = 0;
        while (i < 3) {
            Debug.Log("BasicMove Animation Playing...");
            yield return new WaitForSeconds(1f);
            i++;
        }

        Debug.Log("Move Executed. Ending Coroutine.");
        ClickManager.Instance.allowClicks = true;
        yield break;
    }

    /*private IEnumerator ExecuteMoveCoroutine(Lifeforms unit) {
        yield return unit.StartCoroutine(TargetEnemy(unit));
    }*/

    /*public override void Execute(Lifeforms unit) {
        Debug.Log("Performing move: " + moveName);
        //float damage = CalculateDamage(unit);


    }*/

    /*public override void Execute() {
        Debug.Log("Performing move: " + moveName);
        float damage = CalculateDamage();
    }*/
}