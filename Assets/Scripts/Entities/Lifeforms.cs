using System.Collections;
using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [Header("Moves")]
    private BasicMove basicMove;
    private SpecialMove specialMove;
    private UltimateMove ultimateMove;

    [Header("Actions")]
    protected RecoverAction recoverAction;

    [Header("Stats")]
    [SerializeField] private string unitName;
    [SerializeField] private int actionPoints;
    [SerializeField] private int actionPointRecovery;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMoveDistance;
    [SerializeField] private float defence;

    [SerializeField] protected ActionBase[] moves;
    [SerializeField] protected ActionBase[] actions;

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
        stats.InitializeStats(unitName, actionPoints, actionPointRecovery, maxHealth, maxMoveDistance, defence);
        Debug.Log($"Unit Name: {stats.UnitName}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
        SetMoves();

        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);
    }

    private void Start() {
        /*health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);*/
        MaxActionPoints = stats.ActionPoints;

        CurrentActionPoints = stats.ActionPoints;
        Debug.Log($"Current Action Points: {CurrentActionPoints}");

        //Debug.Log($"Unit Name: {stats.Name}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
    }

    public ActionBase[] GetMoves() {
        return moves;
    }

    public ActionBase[] GetActions() {
        return actions;
    }

    protected virtual void SetMoves() {
        // Moves
        basicMove = moves[0] as BasicMove;
        specialMove = moves[1] as SpecialMove;
        ultimateMove = moves[2] as UltimateMove;

        // Actions
        recoverAction = actions[0] as RecoverAction;
    }

    public void PerformMove(string moveType, Lifeforms target) {
        StartCoroutine(PerformMoveType(moveType, target));
    }

    public IEnumerator PerformMoveType(string moveType, Lifeforms target) {
        Debug.Log($"{target.stats.UnitName} is the target");
        switch (moveType) {
            case "Basic":
                yield return StartCoroutine(basicMove.Execute(this, target));
                stats.SubtractActionPoints(basicMove.GetAPRequirement());
                //CurrentActionPoints -= basicMove.GetAPRequirement();
                break;
            case "Special":
                yield return StartCoroutine(specialMove.Execute(this, target));
                stats.SubtractActionPoints(specialMove.GetAPRequirement());
                //CurrentActionPoints -= specialMove.GetAPRequirement();
                break;
            case "Ultimate":
                yield return StartCoroutine(ultimateMove.Execute(this, target));
                stats.SubtractActionPoints(ultimateMove.GetAPRequirement());
                //CurrentActionPoints -= ultimateMove.GetAPRequirement();
                break;
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
        }
    }

    /*public void PerformMove() {
        specialMove.Execute(this);
    }*/

    public void PerformUltimateMove() {
        ultimateMove.Execute(this);
    }

    public int GetMoveAPRequirement(string moveType) {
        switch (moveType) {
            case "Basic":
                Debug.Log(basicMove);
                return basicMove.GetAPRequirement();
            case "Special":
                return specialMove.GetAPRequirement();
            case "Ultimate":
                return ultimateMove.GetAPRequirement();
            default:
                return 0;
        }
    }

    // Probably dont need this if I make actionPoints into a property
    public int GetActionPoints() {
        Debug.Log($"Current Action Points GETAP: {CurrentActionPoints}");
        return CurrentActionPoints;
    }
}


[System.Serializable]
//[SerializeField]
public class Stats {
    private string unitName;
    private int actionPoints;
    private int actionPointRecovery;
    private float maxHealth;
    private float maxMoveDistance;
    private float defence;

    public void InitializeStats(string unitName, int actionPoints, int actionPointRecovery, float maxHealth, float maxMoveDistance, float defence) {
        UnitName = unitName;
        ActionPoints = actionPoints;
        ActionPointRecovery = actionPointRecovery;
        MaxHealth = maxHealth;
        MaxMoveDistance = maxMoveDistance;
        Defence = defence;
    }

    public string UnitName {
        get { return unitName; }
        private set { unitName = value; }
    }

    public float MaxHealth {
        get { return maxHealth; }
        private set { maxHealth = value; }
    }

    public int ActionPoints {
        get { return actionPoints; }
        private set { actionPoints = value; }
    }

    public int ActionPointRecovery {
        get { return actionPointRecovery; }
        private set { actionPointRecovery = value; }
    }

    public float MaxMoveDistance {
        get { return maxMoveDistance; }
        private set { maxMoveDistance = value; }
    }

    public float Defence {
        get { return defence; }
        private set { defence = value; }
    }

    public void SetActionPoints(int value) {
        ActionPoints = value;
    }

    public void SetActionPointRecovery(int value) {
        ActionPointRecovery = value;
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

}