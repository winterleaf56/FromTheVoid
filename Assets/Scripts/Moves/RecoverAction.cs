using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RecoverAction", menuName = "Actions/Recover Action")]
public class RecoverAction : ActionBase {
    // This action will recover the player's Action Points by a set amount next turn
    // Can only be used if player has not made a move, they can reposition a short distance though.
    [SerializeField] private float recoveryAmount = 50;



    public override IEnumerator Execute(Lifeforms unit) {
        BattleManager.Instance.PlayerRecovering(recoveryAmount);
        return null;
    }

    /*protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        button.GetComponentInChildren<TMP_Text>().SetText("Recover AP");
    }*/
}
