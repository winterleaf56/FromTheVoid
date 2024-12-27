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
    }

    private IEnumerator ExecuteEnemyTurn(Enemy enemy) {
        while (enemy.GetActionPoints() > 0) {
            playerUnitsInRangeOfAttack.Clear();
            // Detect player units in range
            playerUnitsInRange = enemy.DetectAllEnemies(enemy);
            GetEnemiesInRangeOfAttack(enemy);

            Debug.Log($"Player units in range: {playerUnitsInRange.Count}");
            Debug.Log($"Player units in attack range: {playerUnitsInRangeOfAttack.Count}");

            // Attack if a player is already in range
            if (playerUnitsInRangeOfAttack.Count > 0) {
                var attackMove = enemy.GetMoves()
                    .OfType<EnemyBasicAttack>()
                    .FirstOrDefault();

                if (attackMove != null && enemy.GetActionPoints() >= attackMove.GetActionPointCost()) {
                    yield return attackMove.Execute(enemy, playerUnitsInRangeOfAttack[0]);
                    enemy.stats.SubtractActionPoints(attackMove.GetActionPointCost());
                } else {
                    break; // Not enough AP for any attacks
                }
            } else if (playerUnitsInRange.Count > 0) {
                // Move closer to a player unit but not closer than attack range
                Vector3 movePosition = DetermineBestMovePosition(enemy, playerUnitsInRange, enemy.GetMoves().OfType<EnemyBasicAttack>().First().GetRange());
                if (movePosition != Vector3.zero) {
                    var repositionAction = enemy.GetActions()
                        .OfType<EnemyRepositionAction>()
                        .FirstOrDefault();

                    if (repositionAction != null && enemy.GetActionPoints() >= repositionAction.GetActionPointCost()) {
                        repositionAction.SetMovePos(movePosition);
                        yield return repositionAction.Execute(enemy, playerUnitsInRange[0]);
                        enemy.stats.SubtractActionPoints(repositionAction.GetActionPointCost());
                    } else {
                        break; // Not enough AP for movement
                    }
                } else {
                    break; // No valid move positions
                }
            } else {
                break; // No player units in range
            }

            // Check if there are still actions to perform
            if (enemy.GetActionPoints() <= 0) {
                break; // Exit the loop if no AP remains
            }
        }

        Debug.Log("Enemy turn ended.");
    }


    private Vector3 DetermineBestMovePosition(Enemy enemy, List<Friendly> targets, float attackRange) {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        float maxMoveDistance = (enemy.GetActionPoints() / enemy.GetActions().OfType<EnemyRepositionAction>().First().GetActionPointCost()) * navMeshAgent.speed;

        Vector3 bestPosition = Vector3.zero;
        float bestDistance = float.MaxValue;

        foreach (var target in targets) {
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(target.transform.position, path)) {
                float totalPathDistance = NavigationUtils.CalculatePathDistance(navMeshAgent, target.transform.position);

                // Check if the total path distance is greater than the attack range
                if (totalPathDistance > attackRange && totalPathDistance <= maxMoveDistance) {
                    Vector3 positionWithinRange = NavigationUtils.GetPositionWithinRange(path, totalPathDistance - attackRange);
                    float distanceToTarget = Vector3.Distance(positionWithinRange, target.transform.position);

                    if (distanceToTarget >= attackRange && distanceToTarget < bestDistance) {
                        bestPosition = positionWithinRange;
                        bestDistance = distanceToTarget;
                    }
                }
            }
        }

        return bestPosition;
    }


    private void GetEnemiesInRangeOfAttack(Enemy enemy) {
        foreach (EnemyActionBase move in enemy.GetMoves()) {
            if (move != null) {
                foreach (Friendly friendly in move.GetEnemiesInRange(enemy)) {
                    if (friendly != null) {
                        playerUnitsInRangeOfAttack.Add(friendly);
                    }
                }
            }
            
        }
    }

    public IEnumerator StartEnemyTurn(List<GameObject> enemies) {
        Debug.Log("Enemy Turn: Starting Enemy Turn");
        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                Debug.Log($"Enemy Turn: Enemy {enemy.name} is attacking");
                yield return ExecuteEnemyTurn(enemy.GetComponent<Enemy>());
            }
            
            //Lifeforms enemyLifeform = enemy.GetComponent<Lifeforms>();
            
        }
        yield return new WaitForSeconds(1);
        Debug.Log("Enemy Turn: Ending Enemy Turn");
    }

    // Use this later for removing or adding status effects
    private void Attacked(Lifeforms attacker) {

    }
}
