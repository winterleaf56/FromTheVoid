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

[CreateAssetMenu(fileName = "EnemyBasicAttack", menuName = "Enemy Actions/Basic Attack")]
public class EnemyBasicAttack : EnemyActionBase {
    [SerializeField] protected float damage;

    public IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("Performing move: " + moveName + ", against: " + target);

        target.health.TakeDamage(damage, unit);
        BattleManager.audioClip.Invoke(actionSound);

        unit.stats.SubtractActionPoints(actionPointCost);
        Debug.Log($"SUBTRACTED {actionPointCost} AP FROM ENEMY UNIT {unit.stats.UnitName}");
        //OnMoveFinished(unit);

        yield return null;
    }
}

[CreateAssetMenu(fileName = "EnemyRepositionAction", menuName = "Enemy Actions/Reposition")]
public class EnemyRepositionAction : EnemyActionBase {
    [SerializeField] private float distance;

    public Vector3 positionToMoveTo { get; private set;}

    public IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("Performing move: " + moveName + ", against: " + target);

        //unit.transform.position = target.transform.position + target.transform.forward * distance;

        Navigation navigation = unit.GetComponent<Navigation>();
        if (navigation == null) {
            Debug.LogError("No navigation component found");
            yield break;
        }

        bool movementComplete = false;
        navigation.MoveTo(positionToMoveTo, () => {
            movementComplete = true;
            Debug.Log("Movement complete");
        });

        BattleManager.audioClip.Invoke(actionSound);

        while (!movementComplete) {
            yield return null;
        }

        unit.stats.SubtractActionPoints(actionPointCost);
        //OnMoveFinished(unit);

        yield return null;
    }

    public void SetMovePos(Vector3 movePos) {
        positionToMoveTo = movePos;
        Debug.Log($"Move position set to: {positionToMoveTo} in EnemyRepositionAction.");
    }

    

    /*private void SetValues(Vector3 position, GameObject marker, float cost) {
        positionToMoveTo = position;
        confirmButton.GetComponent<Button>().interactable = true;

        placedMarker = marker;

        NavMeshAgent agent = unitToMove.GetComponent<NavMeshAgent>();
        if (agent != null) {
            float pathDistance = NavigationUtils.CalculatePathDistance(agent, position);

            if (pathDistance < Mathf.Infinity) {
                moveCost = pathDistance * actionPointCost;
            } else {
                Debug.LogWarning("Invalid path to the target position.");
                moveCost = Mathf.Infinity;
            }
        } else {
            Debug.LogError("NavMeshAgent not found on the unit.");
            moveCost = Mathf.Infinity;
        }

        moveCost = Mathf.Round(moveCost);

        string stringToSend = $"Movement Cost:\n{moveCost.ToString()} AP";
        UIManager.updateConfirmTxt(stringToSend);
        Debug.Log($"Position: {positionToMoveTo}, Marker: {placedMarker}, Cost: {moveCost}");
    }*/
}