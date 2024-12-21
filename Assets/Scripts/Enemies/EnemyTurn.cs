using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour {
    private int enemyActionPoints;
    private int maxRange;
    private float distanceToPlayerUnit;

    private List<Lifeforms> playerUnitsInRange = new List<Lifeforms>();

    private IEnumerator ExecuteEnemyTurn(Lifeforms enemy) {
        enemyActionPoints = enemy.GetActionPoints();

        foreach (ActionBase move in enemy.GetMoves()) {
            if (move.GetEnemiesInRange(enemy).Count >= 1) {
                yield return move.Execute(enemy);
            } else {
                
            }
        }
    }

    public IEnumerator StartEnemyTurn(GameObject[] enemies) {
        Debug.Log("Enemy Turn: Starting Enemy Turn");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"Enemy Turn: Enemy {enemy.name} is attacking");
            Lifeforms enemyLifeform = enemy.GetComponent<Lifeforms>();
            yield return ExecuteEnemyTurn(enemyLifeform);
        }
        yield return new WaitForSeconds(1);
        Debug.Log("Enemy Turn: Ending Enemy Turn");
    }
}
