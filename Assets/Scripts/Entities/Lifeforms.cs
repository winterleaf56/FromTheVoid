using System.Collections;
using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [Header("Moves")]
    protected BasicMove basicMove;
    protected SpecialMove specialMove;
    protected UltimateMove ultimateMove;

    [Header("Actions")]
    protected RecoverAction recoverAction;

    [SerializeField] protected ActionBase[] moves;
    [SerializeField] protected ActionBase[] actions;

    public Health health;

    //public float damage;

    public Stats stats;

    public bool firstAction { get; private set; }

    public int currentActionPoints { get; private set; }
    public int maxActionPoints { get; private set; }

    public abstract void Attack();

    public abstract void Die();

    public abstract void Damage(float value);


    private void Start() {
        maxActionPoints = stats.ActionPoints;
        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);
        currentActionPoints = stats.ActionPoints;

        Debug.Log($"Unit Name: {stats.Name}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
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
        Debug.Log($"{target.stats.Name} is the target");
        switch (moveType) {
            case "Basic":
                yield return StartCoroutine(basicMove.Execute(this, target));
                currentActionPoints -= basicMove.GetAPRequirement();
                break;
            case "Special":
                yield return StartCoroutine(specialMove.Execute(this, target));
                currentActionPoints -= specialMove.GetAPRequirement();
                break;
            case "Ultimate":
                yield return StartCoroutine(ultimateMove.Execute(this, target));
                currentActionPoints -= ultimateMove.GetAPRequirement();
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
                currentActionPoints -= recoverAction.GetAPRequirement();
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
    public int getActionPoints() {
        return currentActionPoints;
    }
}


[System.Serializable]
public class Stats {
    [SerializeField] string name;
    [SerializeField] private int actionPoints;
    [SerializeField] private int actionPointRecovery;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMoveDistance;
    [SerializeField] private float defence;

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public int ActionPoints {
        get { return actionPoints; }
        set { actionPoints = value; }
    }

    public int ActionPointRecovery {
        get { return actionPointRecovery; }
        set { actionPointRecovery = value; }
    }

    public float MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float MaxMoveDistance {
        get { return maxMoveDistance; }
        set { maxMoveDistance = value; }
    }

    public float Defence {
        get { return defence; }
        set { defence = value; }
    }

}