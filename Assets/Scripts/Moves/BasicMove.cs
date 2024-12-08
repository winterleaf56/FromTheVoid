using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Basic Attack")]
public class BasicMove : ActionBase {
    //private new GameObject target;

    /*[SerializeField] private GameObject confirmPage;
    [SerializeField] private GameObject confirmBtn;*/

    public virtual float CalculateDamage() {
        return damage;
    }

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            BattleManager.Instance.AttackingToggle();
            OnClickedBasic(confirmPage, confirmBtn, cancelBtn);
            Debug.Log("Executing basic move by setting up button");
        });

    }

    protected override void ConfigureButton(Button button, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
    }

    // OnClickedBasic is OnClicked but toggles BattleState to PlayerAttack so children can override OnClicked and not have to worry about toggling BattleState to PlayerAttack
    protected virtual void OnClickedBasic(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.OnClicked(confirmPage, confirmBtn, cancelBtn);

        cancelBtn.onClick.AddListener(() => {
            BattleManager.Instance.AttackingToggle();
        });
    }

    // Make a coroutine that runs while the player is selecting an enemy to attack, then when the attack occurs, the coroutine ends
    public override IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("DAMAGE: " + damage);
        //Debug.Log("Performing move: " + moveName);
        Debug.Log($"Performing move: {moveName}, against: {target}");

        // Damage the target
        target.GetComponentInParent<Health>().TakeDamage(CalculateDamage());
        Debug.Log($"Damaging {target} for {CalculateDamage()} damage.");

        // Subtract action points for executing move
        unit.stats.ActionPoints -= actionPoints;

        int i = 0;
        while (i < 3) {
            Debug.Log("BasicMove Animation Playing...");
            yield return new WaitForSeconds(1f);
            i++;
        }

        Debug.Log("Move Executed. Ending Coroutine.");
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