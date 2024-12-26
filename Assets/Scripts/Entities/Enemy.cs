using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Lifeforms {
    [SerializeField] private float viewRange;

    [SerializeField] private LayerMask obstacleLayer;

    protected EnemyBasicAttack enemyBasicMove;

    [Header("Enemy Type")]
    [SerializeField] private Enemy enemyType;

    [SerializeField] protected EnemyActionBase[] enemyMoves;
    [SerializeField] protected EnemyActionBase[] enemyActions;
    [SerializeField] public Lifeforms currentTarget { get; private set; }

    private NavMeshAgent navMeshAgent;

    private void OnEnable() {
        obstacleLayer = LayerMask.GetMask("Wall");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Awake() {
        FirstStart();
    }

    public new EnemyActionBase[] GetMoves() {
        return enemyMoves;
    }

    public new EnemyActionBase[] GetActions() {
        return enemyActions;
    }

    public List<Friendly> GetEnemiesInRange(Lifeforms unit) {
        List<Friendly> enemiesInRange = new List<Friendly>();
        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, viewRange);
        float closestDistance = float.MaxValue;
        Friendly closestEnemyUnit = null;

        foreach (var hitCollider in hitColliders) {
            Friendly enemy = hitCollider.GetComponent<Friendly>();
            if (enemy != null) {
                if (!Physics.Linecast(unit.transform.position, enemy.transform.position, out RaycastHit hit, obstacleLayer)) {
                    enemiesInRange.Add(enemy);

                    float distance = NavigationUtils.CalculatePathDistance(navMeshAgent, enemy.transform.position);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemyUnit = enemy;
                    }
                }
            }
        }

        if (closestEnemyUnit != null) {
            SetTarget(closestEnemyUnit);
        }

        return enemiesInRange;
    }

    public List<Friendly> DetectAllEnemies(Lifeforms unit) {
        List<Friendly> enemiesInRange = new List<Friendly>();
        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, viewRange);
        float closestDistance = float.MaxValue;
        Friendly closestEnemyUnit = null;

        foreach (var hitCollider in hitColliders) {
            Friendly enemy = hitCollider.GetComponent<Friendly>();
            if (enemy != null) {
                if (!Physics.Linecast(unit.transform.position, enemy.transform.position, out RaycastHit hit)) {
                    enemiesInRange.Add(enemy);

                    float distance = NavigationUtils.CalculatePathDistance(navMeshAgent, enemy.transform.position);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemyUnit = enemy;
                    }
                }
            }
        }

        if (closestEnemyUnit != null) {
            SetTarget(closestEnemyUnit);
        }

        return enemiesInRange;
    }

    public void SetTarget(Friendly target) {
        currentTarget = target.GetComponent<Lifeforms>();
        Debug.Log($"Enemy {name} has set target to {target.name}");
    }



    public override void Attack() {
        throw new System.NotImplementedException();
    }

    public override void Die() {
        throw new System.NotImplementedException();
    }

    public override void Damage(float value) {
        throw new System.NotImplementedException();
    }

    /*public override void OnMouseDown() {
        Debug.Log("This is an enemy unit!");

        // If current state is the enemy's turn, display the enemy's stats
        if (BattleManager.Instance.currentTurn == BattleManager.GameState.EnemyTurn) {
            Debug.Log("Displaying Enemy Stats");
        } else if (BattleManager.Instance.currentTurn == BattleManager.GameState.PlayerTurn) {
            // If the current state is the player's turn, invoke an action on the enemy (check if an action is selected)
            Debug.Log("Player invoking action on enemy");
        }
    }*/

}
