using UnityEngine;
using System.Collections;

public class Assault : Friendly {
    [Header("Actions")]
    [SerializeField] protected StaminaStimAction staminaStimAction;

    protected override void SetMoves() {
        //base.SetMoves();

        // Currently overrideing because there is only 1 move for Assault
        basicMove = moves[0] as BasicMove;

        recoverAction = actions[0] as RecoverAction;
        repositionAction = actions[1] as RepositionAction;
        //staminaStimAction = actions[1] as StaminaStimAction;
    }

    /*public override IEnumerator PerformActionType(string actionType, Lifeforms target) {
        base.PerformActionType(actionType, target);

        switch (actionType) {
            case "Stamina":
                yield return StartCoroutine(staminaStimAction.Execute(this));
                target.stats.SubtractActionPoints(staminaStimAction.GetAPRequirement());
                //target.stats.ActionPoints -= recoverAction.GetAPRequirement();
                break;
        }

        yield break;
    }*/
}
