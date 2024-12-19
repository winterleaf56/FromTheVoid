using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "StaminaStimAction", menuName = "Actions/Stamina Stim Action")]
public class StaminaStimAction : ActionBase {
    private bool actionActive = false;

    [SerializeField] private float moveMultiplier = 1.2f;
    [SerializeField] private int recoverActionPoints = 25;
    [SerializeField] private int turnsActive = 2;
    private int disableOnTurn;
    private int currentTurn;

    /*private void OnEnable() {
        currentTurn = BattleManager.Instance.turnNumber;
    }*/

    void Update() {
        currentTurn = BattleManager.Instance.turnNumber;
    }

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.SetupButton(button, unit, confirmPage, confirmBtn, cancelBtn);
        //button.onClick.AddListener(() => BattleManager.Instance.AttackingToggle());
        /*confirmPage.SetActive(true);
        confirmBtn.SetActive(true);
        confirmBtn.gameObject.GetComponent<Button>().onClick.AddListener(() => PlayerTurn.Instance.StaminaStimAction());*/
        //button.onClick.AddListener(() => PlayerTurn.Instance.BasicMove());
        button.GetComponentInChildren<TMP_Text>().SetText("Stamina Stim");
    }

    protected override void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        base.ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }

    public override IEnumerator Execute(Lifeforms unit) {
        //StatusEffectManager.Instance.AddEffect(StatusEffectManager.StatusEffectType.ActionPointEffect, "StaminaStimEffect", 2, 25, unit);

        yield break;
    }

    /*public override IEnumerator Execute(Lifeforms unit) {
        actionActive = true;
        disableOnTurn = currentTurn + turnsActive;

        unit.stats.SetActionPointRecovery(unit.stats.ActionPointRecovery + recoverActionPoints) ;
        unit.stats.SetMaxMoveDistance(unit.stats.MaxMoveDistance * moveMultiplier);

        BattleManager.Instance.ToggleButton("Stamina", false);

        unit.StartCoroutine(EndAction(unit));

        Debug.Log("Applying Stamina Stim");

        yield break;
    }

    private IEnumerator EndAction(Lifeforms unit) {
        while (currentTurn <= disableOnTurn) {
            yield return null;
        }

        unit.stats.SetActionPointRecovery(unit.stats.ActionPointRecovery - recoverActionPoints);
        unit.stats.SetMaxMoveDistance(unit.stats.MaxMoveDistance / moveMultiplier);

        BattleManager.Instance.ToggleButton("Stamina", true);

        Debug.Log("Stamina Stim Has Worn Off");

        actionActive = false;
        yield break;
    }*/

}
