using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [Header("Moves")]
    protected BasicMove basicMove;
    protected SpecialMove specialMove;
    //protected UltimateMove ultimateMove;

    [Header("Actions")]
    protected RecoverAction recoverAction;
    protected RepositionAction repositionAction;

    [Header("Stats")]
    [SerializeField] private string unitName;
    [SerializeField] private int actionPoints;
    [SerializeField] private int actionPointRecovery;
    [SerializeField] private int ultimatePoints;
    [SerializeField] private int maxUltimatePoints;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMoveDistance;
    [SerializeField] private float defence;

    [SerializeField] protected ActionBase[] moves;
    [SerializeField] protected ActionBase[] actions;

    [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    public Health health;

    //public static Stats stats;

    //public float damage;

    public Stats stats = new Stats();

    public bool firstAction { get; private set; }

    public int CurrentActionPoints { get; private set; }
    public int MaxActionPoints { get; private set; }

    public abstract void Attack();

    public abstract void Die();

    public abstract void Damage(float value);

    private void Awake() {
        /*Debug.Log("Loading for the first time");
        stats.InitializeStats(unitName, actionPoints, actionPointRecovery, ultimatePoints, maxUltimatePoints, maxHealth, maxMoveDistance, defence);
        Debug.Log($"Unit Name: {stats.UnitName}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
        if (GetComponent<Friendly>()) SetMoves();

        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);*/
    }

    private void Start() {
        Debug.Log($"Unit Name: {stats.UnitName}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints} IN LIFEFORMS START");

        /*health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);*/
        MaxActionPoints = stats.ActionPoints;

        CurrentActionPoints = stats.ActionPoints;

        //Debug.Log($"Unit Name: {stats.Name}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
    }

    protected void FirstStart() {
        Debug.Log("Loading for the first time");
        stats.InitializeStats(unitName, actionPoints, actionPointRecovery, ultimatePoints, maxUltimatePoints, maxHealth, maxMoveDistance, defence);
        Debug.Log($"Unit Name: {stats.UnitName}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
        if (GetComponent<Friendly>()) SetMoves();

        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);
    }

    public ActionBase[] GetMoves() {
        return moves;
    }

    public ActionBase[] GetActions() {
        return actions;
    }

    public List<StatusEffect> GetStatusEffects() {
        return statusEffects;
    }

    public void AddStatusEffect(StatusEffect statusEffect) {
        statusEffects.Add(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect) {
        statusEffects.Remove(statusEffect);
    }

    protected virtual void SetMoves() {
        // Moves
        basicMove = moves[0] as BasicMove;
        specialMove = moves[1] as SpecialMove;
        //ultimateMove = moves[2] as UltimateMove;

        // Actions
        recoverAction = actions[0] as RecoverAction;
        repositionAction = actions[1] as RepositionAction;
    }

    public void PerformMove(string moveType, Lifeforms target) {
        StartCoroutine(PerformMoveType(moveType, target));
    }

    public void PerformDirectAttack(ActionBase move, Lifeforms target) {
        StartCoroutine(move.Execute(this, target));
    }

    /*public IEnumerator PerformAreaAttack(ActionBase move, Vector3 target) {
        yield return StartCoroutine(move.Execute(this, target));
    }*/

    public void PerformAction(ActionBase action) {
        StartCoroutine(action.Execute(this));
    }

    public void PerformReposition(RepositionAction reposition) {
        StartCoroutine(reposition.Execute(this));
    }

    /*public IEnumerator PerformAction(ActionBase action) {
        yield return StartCoroutine(action.Execute(this));
    }*/

    public IEnumerator PerformMoveType(string moveType, Lifeforms target) {
        Debug.Log($"{target.stats.UnitName} is the target");
        switch (moveType) {
            case "Basic":
                yield return StartCoroutine(basicMove.Execute(this, target));
                break;
            case "Special":
                yield return StartCoroutine(specialMove.Execute(this, target));
                break;
/*            case "Ultimate":
                yield return StartCoroutine(ultimateMove.Execute(this, target));
                break;*/
            default:
                Debug.Log("Misspelled Move Type or Invalid Type");
                break;
        }
        //basicMove.Execute(this);
        //StartCoroutine(basicMove.Execute(this));
        Debug.Log($"Move: {basicMove.moveName} completed");
        yield break;
    }

    public void PerformAction(string moveType, Lifeforms target) {
        StartCoroutine(PerformActionType(moveType, target));
    }

    public virtual IEnumerator PerformActionType(string actionType, Lifeforms target) {
        switch (actionType) {
            case "Recover":
                yield return StartCoroutine(recoverAction.Execute(this));
                CurrentActionPoints -= recoverAction.GetAPRequirement();
                break;
            case "Reposition":
                yield return StartCoroutine(repositionAction.Execute(this));
                CurrentActionPoints -= repositionAction.GetAPRequirement();
                break;
        }
    }

    /*public void PerformMove() {
        specialMove.Execute(this);
    }*/

    public void PerformUltimateMove() {
        //ultimateMove.Execute(this);
    }

    public void StartRound() {
        RecoverAP();
        TakeDamage();
    }

    public void RecoverAP() {
        int recoveryAmount = (stats.ActionPoints + stats.ActionPointRecovery > stats.MaxActionPoints) ? stats.MaxActionPoints - stats.ActionPoints : stats.ActionPointRecovery;

        stats.AddActionPoints(recoveryAmount);

        Debug.Log($"Recovering {recoveryAmount} AP");
    }

    public void TakeDamage() {
        if (stats.DamageOverTime > 0) {
            health.TakeDamage(stats.DamageOverTime, this);
            Debug.Log($"Taking {stats.DamageOverTime} damage");
        }
    }

    public int GetMoveAPRequirement(string moveType) {
        switch (moveType) {
            case "Basic":
                Debug.Log(basicMove);
                return basicMove.GetAPRequirement();
            case "Special":
                return specialMove.GetAPRequirement();
            /*case "Ultimate":
                return ultimateMove.GetAPRequirement()*/;
            default:
                return 0;
        }
    }

    // Probably dont need this if I make actionPoints into a property
    public int GetActionPoints() {
        Debug.Log($"Current Action Points GETAP: {stats.ActionPoints}");
        return stats.ActionPoints;
    }
}


[System.Serializable]
//[SerializeField]
public class Stats {
    /*private string unitName;
    private string unitType;
    private int actionPoints;
    private int maxActionPoints;
    private int actionPointRecovery;
    private int ultimatePoints;
    private int maxUltimatePoints;
    private float maxHealth;
    private float healOverTime;
    private float maxMoveDistance;
    private float defence;*/

    public string UnitName { get; private set; }
    public string UnitType { get; private set; }
    public int ActionPoints { get; private set; }
    public int MaxActionPoints { get; private set; }
    public int ActionPointRecovery { get; private set; }
    public int UltimatePoints { get; private set; }
    public int MaxUltimatePoints { get; private set; }
    public float MaxHealth { get; private set; }
    public float HealOverTime { get; private set; }
    public float DamageOverTime { get; private set; }
    public float MaxMoveDistance { get; private set; }
    public float Defence { get; private set; }

    public void InitializeStats(string unitName, int actionPoints, int actionPointRecovery, int ultimatePoints, int maxUltimatePoints, float maxHealth, float maxMoveDistance, float defence) {
        UnitName = unitName;
        ActionPoints = actionPoints;
        MaxActionPoints = actionPoints;
        ActionPointRecovery = actionPointRecovery;
        UltimatePoints = ultimatePoints;
        MaxUltimatePoints = maxUltimatePoints;
        MaxHealth = maxHealth;
        MaxMoveDistance = maxMoveDistance;
        Defence = defence;
    }


    /*public string UnitName {
        get { return unitName; }
        private set { unitName = value; }
    }

    // Maybe make types classes or enums and when damage is dealt, send it through an Action and the corresponding type can mitigate or amplify the damage
    public string UnitType {
        get { return unitType; }
        private set { unitType = value; }
    }

    public float MaxHealth {
        get { return maxHealth; }
        private set { maxHealth = value; }
    }

    public float HealOverTime {
        get { return healOverTime; }
        private set { healOverTime = value; }
    }

    public int ActionPoints {
        get { return actionPoints; }
        private set { actionPoints = value; }
    }

    public int MaxActionPoints {
        get { return maxActionPoints; }
        private set { }
    }

    public int ActionPointRecovery {
        get { return actionPointRecovery; }
        private set { actionPointRecovery = value; }
    }

    public int UltimatePoints {
        get { return ultimatePoints; }
        private set { ultimatePoints = value; }
    }

    public int MaxUltimatePoints {
        get { return maxUltimatePoints; }
        private set { maxUltimatePoints = value; }
    }

    public float MaxMoveDistance {
        get { return maxMoveDistance; }
        private set { maxMoveDistance = value; }
    }

    public float Defence {
        get { return defence; }
        private set { defence = value; }
    }*/

    public void SetActionPoints(int value) {
        ActionPoints = value;
    }

    public void AddActionPointRecovery(int value) {
        ActionPointRecovery += value;
    }

    public void AddActionPoints(int value) {
        ActionPoints += value;
    }

    public void SubtractActionPoints(int value) {
        ActionPoints -= value;
    }

    public void SetMaxMoveDistance(float value) {
        MaxMoveDistance = value;
    }

    public void SetDefence(float value) {
        Defence = value;
    }

    public void AddUltimatePoints(int value) {
        UltimatePoints += value;
    }

    public void SetHealOverTime(float value) {
        HealOverTime = value;
    }

    public void SetDamageOverTime(float value) {
        DamageOverTime = value;
    }

}