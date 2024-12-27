using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyActionBase : ScriptableObject {
    [SerializeField] protected string moveName;
    [SerializeField] protected int actionPointCost;
    [SerializeField] protected float range;

    [SerializeField] protected AudioClip actionSound;

    [SerializeField] protected LayerMask obstacleLayer;

    private void OnEnable() {
        obstacleLayer = LayerMask.GetMask("Wall");
    }

    public List<Friendly> GetEnemiesInRange(Lifeforms unit) {
        List<Friendly> enemiesInRange = new List<Friendly>();
        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, range);

        foreach (var hitCollider in hitColliders) {
            Friendly enemy = hitCollider.GetComponent<Friendly>();
            if (enemy != null) {
                if (!Physics.Linecast(unit.transform.position, enemy.transform.position, out RaycastHit hit, obstacleLayer)) {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        return enemiesInRange;
    }

    public int GetActionPointCost() {
        return actionPointCost;
    }

    public float GetRange() {
        return range;
    }
}