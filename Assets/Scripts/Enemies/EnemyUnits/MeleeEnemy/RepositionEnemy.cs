using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "EnemyRepositionAction", menuName = "Enemy Actions/Reposition")]
public class EnemyRepositionAction : EnemyActionBase {
    [SerializeField] private float distance;

    public Vector3 positionToMoveTo { get; private set; }

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

        BattleManager.audioClip.Invoke(actionSound, unit.transform.position);

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
}