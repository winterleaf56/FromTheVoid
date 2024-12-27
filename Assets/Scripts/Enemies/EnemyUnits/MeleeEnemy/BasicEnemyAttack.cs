using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyBasicAttack", menuName = "Enemy Actions/Basic Attack")]
public class EnemyBasicAttack : EnemyActionBase {
    [SerializeField] protected float damage;

    public IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("Performing move: " + moveName + ", against: " + target);

        target.health.TakeDamage(damage, unit);
        BattleManager.audioClip.Invoke(actionSound, unit.transform.position);

        unit.stats.SubtractActionPoints(actionPointCost);
        Debug.Log($"SUBTRACTED {actionPointCost} AP FROM ENEMY UNIT {unit.stats.UnitName}");
        //OnMoveFinished(unit);

        yield return null;
    }
}