using System.Collections;
using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [Header("Moves")]
    private BasicMove basicMove;
    private SpecialMove specialMove;
    private UltimateMove ultimateMove;

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
        stats.InitializeStats(unitName, actionPoints, actionPointRecovery, ultimatePoints, maxUltimatePoints, maxHealth, maxMoveDistance, defence);
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
            case "Ultimate":
                yield return StartCoroutine(ultimateMove.Execute(this, target));
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
    private int ultimatePoints;
    private int maxUltimatePoints;
    private float maxHealth;
    private float maxMoveDistance;
    private float defence;

    public void InitializeStats(string unitName, int actionPoints, int actionPointRecovery, int ultimatePoints, int maxUltimatePoints, float maxHealth, float maxMoveDistance, float defence) {
        UnitName = unitName;
        ActionPoints = actionPoints;
        ActionPointRecovery = actionPointRecovery;
        UltimatePoints = ultimatePoints;
        MaxUltimatePoints = maxUltimatePoints;
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

    public void AddUltimatePoints(int value) {
        UltimatePoints += value;
    }

}