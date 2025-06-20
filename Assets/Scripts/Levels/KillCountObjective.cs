using UnityEngine;

[CreateAssetMenu(fileName = "KillCountObjective", menuName = "Objective/Kill Count Objective")]
public class KillCountObjective : ObjectiveBase {
    public int targetCount;

    public override void CheckObjective() {
        if (BattleManager.Instance.DeadEnemyUnits.Count >= targetCount) {
            isCompleted = true;
        }
    }
}
