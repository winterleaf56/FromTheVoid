using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Special Attack")]
public class SpecialMove : BasicMove {
    [SerializeField] private float cooldown;

    public Action moveSelected;

    /*public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
    }*/

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            OnClickedBasic(confirmPage, confirmBtn, cancelBtn);
            Debug.Log("Executing Special move by setting up button");
        });

    }

    protected override void ConfigureButton(Button button, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
    }


}
