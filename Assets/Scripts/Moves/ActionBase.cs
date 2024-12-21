using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting.FullSerializer;
using TMPro;
using static UnityEngine.UI.CanvasScaler;
using System;

public abstract class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] protected int actionPointCost;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button actionButton;

    [SerializeField] protected LayerMask obstacleLayer;

    protected GameObject rangeRing;

    //protected BattleManager BattleManager => BattleManager.Instance;

    //public UnityAction moveFinishedEvent;

    //public UnityAction<List<Enemy>> attackableEnemies;

    // Makes more sense to remove this replace it with an image/sprite maybe.
    [SerializeField] public GameObject buttonPrefab;


    protected bool isAOE;

    private void OnEnable() {
        obstacleLayer = LayerMask.GetMask("Wall");
    }

    public virtual void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        button.interactable = true;
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }

    protected virtual void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        string formattedName = moveName.Replace(" ", "\n");
        button.GetComponentInChildren<TMP_Text>().SetText(formattedName);

        if (unit.stats.ActionPoints < GetAPRequirement()) {
            DisableButton(button);
        }

        button.transform.Find("CostTxt").GetComponent<TMP_Text>().SetText($"{GetAPRequirement().ToString()} AP");
    }

    protected virtual void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        Button confBtn = confirmBtn.gameObject.GetComponent<Button>();
        confBtn.onClick.AddListener(() => {
            confirmPage.SetActive(false);
        });
    }

    protected void DisableButton(Button button) {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;

        button.interactable = false;
        colors.normalColor = new Color(0.5f, 0.5f, 0.5f);
    }

    public virtual IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("Performing move: " + moveName + ", against: " + target);

        OnMoveFinished(unit);

        yield return null;
    }

    public virtual IEnumerator Execute(Lifeforms unit) {
        Debug.Log("Performing move: " + moveName);

        OnMoveFinished(unit);

        yield return null;
    }

    public List<Enemy> GetEnemiesInRange(Lifeforms unit) {
        List<Enemy> enemiesInRange = new List<Enemy>();
        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, range);

        foreach  (var hitCollider in hitColliders) {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null) {
                if (!Physics.Linecast(unit.transform.position, enemy.transform.position, out RaycastHit hit, obstacleLayer)) {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        Debug.Log(enemiesInRange);
        return enemiesInRange;
    }

    protected virtual void OnMoveFinished(Lifeforms unit) {
        PlayerTurn.Instance.changedAP?.Invoke();
        BattleManager.onMoveFinished?.Invoke();
    }

    protected virtual void OnMoveFinished() {
        PlayerTurn.Instance.changedAP?.Invoke();
        BattleManager.changeBattleState?.Invoke(BattleManager.BattleState.PlayerIdle);
    }

    public int GetAPRequirement() {
        return actionPointCost;
    }
}
