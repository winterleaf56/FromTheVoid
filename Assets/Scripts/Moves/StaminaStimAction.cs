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

    public override void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn) {
        base.SetupButton(button, unit, confirmPage, confirmBtn);
        //button.onClick.AddListener(() => BattleManager.Instance.AttackingToggle());
        confirmPage.SetActive(true);
        confirmBtn.SetActive(true);
        confirmBtn.gameObject.GetComponent<Button>().onClick.AddListener(() => PlayerTurn.Instance.StaminaStimAction());
        //button.onClick.AddListener(() => PlayerTurn.Instance.BasicMove());
    }

    public override IEnumerator Execute(Lifeforms unit) {
        actionActive = true;
        disableOnTurn = currentTurn + turnsActive;

        unit.stats.ActionPointRecovery += recoverActionPoints;
        unit.stats.MaxMoveDistance *= moveMultiplier;

        BattleManager.Instance.ToggleButton("Stamina", false);

        unit.StartCoroutine(EndAction(unit));

        Debug.Log("Applying Stamina Stim");

        yield break;
    }

    private IEnumerator EndAction(Lifeforms unit) {
        while (currentTurn <= disableOnTurn) {
            yield return null;
        }

        unit.stats.ActionPointRecovery -= recoverActionPoints;
        unit.stats.MaxMoveDistance /= moveMultiplier;

        BattleManager.Instance.ToggleButton("Stamina", true);

        Debug.Log("Stamina Stim Has Worn Off");

        actionActive = false;
        yield break;
    }

}
