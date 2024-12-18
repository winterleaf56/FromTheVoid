using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Special Attack")]
public class SpecialMove : BasicMove {
    [SerializeField] private float cooldown;

    public Action moveSelected;

    /*public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
    }*/

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        //button.GetComponentInChildren<TMP_Text>().SetText("Special Attack");
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerAttack);
            OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn, rangeRing);
            Debug.Log("Executing Special move by setting up button");
        });

    }

    protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }


}
