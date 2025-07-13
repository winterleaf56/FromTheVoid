using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Action", menuName = "Actions/Medic/Heal Friendly")]
public class HealFriendly : FriendlyTargetMove {

    // This class is a basic healing action. Click to heal the selected friendly unit.
    // More advanced heals can be created by inheriting from this class.

    public virtual float CalculateHeal() {
        return value;
    }

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerAction);
            //OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn, rangeRing);
            Debug.Log("Executing basic move by setting up button");
        });

    }
}
