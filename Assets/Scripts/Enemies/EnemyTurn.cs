using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTurn : MonoBehaviour {
    private int enemyActionPoints;
    private int maxRange;
    private float distanceToPlayerUnit;
    private int attackCost;

    private List<Friendly> playerUnitsInRange = new List<Friendly>();
    private List<Friendly> playerUnitsInRangeOfAttack = new List<Friendly>();

    private void Start() {
        Health health = GetComponent<Health>();
        //health.onAttacked += Attacked;
    }

    private IEnumerator ExecuteEnemyTurn(Lifeforms enemy) {
        enemyActionPoints = enemy.GetActionPoints();

        playerUnitsInRange = enemy.GetComponent<Enemy>().DetectAllEnemies(enemy);
        GetEnemiesInRangeOfAttack(enemy.GetComponent<Enemy>());

        if (playerUnitsInRange.Count >= 1) {
            if (playerUnitsInRangeOfAttack.Count >= 1) {
                yield return enemy.GetMoves().OfType<EnemyBasicAttack>().First().Execute(enemy, playerUnitsInRangeOfAttack[0]);
            } else {
                // Get a position to move to
                Vector3 movePosition = DetermineBestMovePosition(enemy, playerUnitsInRange);
                if (movePosition != Vector3.zero) {
                    EnemyRepositionAction repositionAction = enemy.GetActions().OfType<EnemyRepositionAction>().First();
                    repositionAction.SetMovePos(movePosition);
                    yield return repositionAction.Execute(enemy, playerUnitsInRange[0]);
                }

                
                //repositionAction.SetMovePos();

                //yield return enemy.GetActions().OfType<EnemyRepositionAction>().First().Execute(enemy, playerUnitsInRange[0]);
            }

            if (enemyActionPoints >= attackCost) {
                //yield return enemy.GetMoves()[0].Execute(enemy, playerUnitsInRange[0]);
            }
        }

        
    }

    private Vector3 DetermineBestMovePosition(Lifeforms enemy, List<Friendly> targets) {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        float maxMoveDistance = (enemy.GetActionPoints() / enemy.GetActions().OfType<EnemyRepositionAction>().First().GetActionPointCost()) * navMeshAgent.speed;

        Vector3 bestPosition = Vector3.zero;
        float bestDistance = float.MaxValue;

        foreach (var target in targets) {
            float pathDistance = NavigationUtils.CalculatePathDistance(navMeshAgent, target.transform.position);

            if (pathDistance <= maxMoveDistance && pathDistance < bestDistance) {
                bestDistance = pathDistance;

                NavMeshPath path = new NavMeshPath();
                if (navMeshAgent.CalculatePath(target.transform.position, path)) {
                    bestPosition = NavigationUtils.GetPositionWithinRange(path, maxMoveDistance);
                }
            }
        }
        return bestPosition;

    }

    private void GetEnemiesInRangeOfAttack(Enemy enemy) {
        foreach (EnemyActionBase move in enemy.GetMoves()) {
            foreach (Friendly friendly in move.GetEnemiesInRange(enemy)) {
                playerUnitsInRangeOfAttack.Add(friendly);
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

    // Use this later for removing or adding status effects
    private void Attacked(Lifeforms attacker) {

    }
}
