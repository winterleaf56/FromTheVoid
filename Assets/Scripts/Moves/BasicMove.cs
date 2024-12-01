using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Basic Attack")]
public class BasicMove : ActionBase {
    //private new GameObject target;

    public virtual float CalculateDamage() {
        return damage;
    }

    // Make a coroutine that runs while the player is selecting an enemy to attack, then when the attack occurs, the coroutine ends
    public override IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("DAMAGE: " + damage);
        //Debug.Log("Performing move: " + moveName);
        Debug.Log($"Performing move: {moveName}, against: {target}");

        target.GetComponentInParent<Health>().TakeDamage(CalculateDamage());
        Debug.Log($"Damaging {target} for {CalculateDamage()} damage.");

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