using UnityEngine;

[CreateAssetMenu(fileName = "KillCountObjective", menuName = "Objective/Kill Count Objective")]
public class KillCountObjective : ObjectiveBase {
    public int targetCount;

    public override void CheckObjective() {
        Debug.Log($"Checking KillCountObjective: DeadEnemyUnits.Count = {BattleManager.Instance.DeadEnemyUnits.Count}, targetCount = {targetCount}");

        if (BattleManager.Instance.DeadEnemyUnits.Count >= targetCount) {
            isCompleted = true;
            Debug.Log("KillCountObjective completed!");
        }
    }
}
