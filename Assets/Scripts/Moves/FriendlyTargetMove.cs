using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyTargetMove : ActionBase {

    // For future me:
    // This class is what will be used for moves that target friendly units
    // This includes healing, buffs, and other supportive actions

    /*public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        button.onClick.AddListener(() => {
            //BattleManager.Instance.AttackingToggle();
            BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerAttack);
            //OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn, rangeRing);
            Debug.Log("Executing basic move by setting up button");
        });

    }*/

    protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);

        rangeRing = unit.transform.Find("RangeRing").gameObject;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            List<Friendly> friendlyList = GetFriendliesInRange(unit);
            confirmPage.gameObject.SetActive(true);
            PlayerTurn.Instance.SetTargetableFriendlies(friendlyList);
            BattleManager.manageFriendlyLights?.Invoke(friendlyList);

            rangeRing.SetActive(true);
            rangeRing.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
            // Create a method in PlayerTurn to set targetable friendlies like SetAttackableEnemies
            //PlayerTurn.Instance.SetTargetableFriendlies(friendlyList);
        });
    }

    /*protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);


        rangeRing = unit.transform.Find("RangeRing").gameObject;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            List<Enemy> enemyList = GetEnemiesInRange(unit);
            confirmPage.gameObject.SetActive(true);
            PlayerTurn.Instance.SetAttackableEnemies(enemyList);
            BattleManager.manageEnemyLights?.Invoke(enemyList);

            rangeRing.SetActive(true);
            rangeRing.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);

            OnClickedBasic(unit, confirmPage, confirmBtn, cancelBtn, rangeRing);
        });


    }*/
}
