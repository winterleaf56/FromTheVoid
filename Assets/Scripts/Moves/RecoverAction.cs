using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoverAction", menuName = "Actions/Recover Action")]
public class RecoverAction : ActionBase {
    // This action will recover the player's Action Points by a set amount next turn
    // Can only be used if player has not made a move, they can reposition a short distance though.
    [SerializeField] private float recoveryAmount = 50;

    public override IEnumerator Execute(Lifeforms unit) {
        BattleManager.Instance.PlayerRecovering(recoveryAmount);
        return base.Execute(unit);
    }
}