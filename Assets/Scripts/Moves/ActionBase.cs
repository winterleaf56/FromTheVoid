using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting.FullSerializer;
using TMPro;

public abstract class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] protected int actionPointCost;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;

    [SerializeField] protected LayerMask obstacleLayer;

    private BattleManager BattleManager => BattleManager.Instance;

    //public UnityAction moveFinishedEvent;

    //public UnityAction<List<Enemy>> attackableEnemies;

    // Makes more sense to remove this replace it with an image/sprite maybe.
    [SerializeField] public GameObject buttonPrefab;


    protected bool isAOE;

    private void OnEnable() {
        obstacleLayer = LayerMask.GetMask("Wall");
    }

    public virtual void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }

    protected virtual void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        string formattedName = moveName.Replace(" ", "\n");
        button.GetComponentInChildren<TMP_Text>().SetText(formattedName);
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;

        if (unit.stats.ActionPoints < GetAPRequirement()) {
            button.interactable = false;
            colors.normalColor = new Color(0.5f, 0.5f, 0.5f);
        }

        button.transform.Find("CostTxt").GetComponent<TMP_Text>().SetText($"{GetAPRequirement().ToString()} AP");

        button.onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        List<Enemy> enemyList = GetEnemiesInRange(unit);
        GameObject rangeRing = unit.transform.Find("RangeRing").gameObject;

        button.onClick.AddListener(() => {
            confirmPage.gameObject.SetActive(true);
            PlayerTurn.Instance.SetAttackableEnemies(enemyList);
            BattleManager.Instance.ManageLights(enemyList);

            rangeRing.SetActive(true);
            rangeRing.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        });

        cancelBtn.GetComponent<Button>().onClick.AddListener(() => {
            //BattleManager.Instance.ManageLights(enemyList);
            Debug.Log("Canceling move");
            confirmPage.gameObject.SetActive(false);

            rangeRing.SetActive(false);
        });
    }

    protected virtual void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        Button confBtn = confirmBtn.gameObject.GetComponent<Button>();
        confBtn.onClick.AddListener(() => {
            confirmPage.SetActive(false);
            confirmBtn.SetActive(false);
            //PlayerTurn.Instance.BasicMove();
        });
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

        return enemiesInRange;
    }

    protected void OnMoveFinished(Lifeforms unit) {
        //MoveFinishedEvent?.Invoke();
        GameObject rangeRing = unit.transform.Find("RangeRing").gameObject;
        rangeRing.SetActive(false);

        BattleManager.onMoveFinished?.Invoke();
        //BattleManager.changeBattleState.Invoke(BattleManager.BattleState.PlayerIdle);
        BattleManager.ManageLights(GetEnemiesInRange(unit));
    }

    public int GetAPRequirement() {
        return actionPointCost;
    }
}
