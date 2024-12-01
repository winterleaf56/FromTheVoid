using System.Collections;
using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [SerializeField] protected BasicMove basicMove;
    [SerializeField] protected SpecialMove specialMove;
    [SerializeField] protected UltimateMove ultimateMove;

    public Health health;

    //public float damage;

    public Stats stats;

    private float actionPoints;

    public abstract void Attack();

    public abstract void Die();

    public abstract void Damage(float value);

    /*public float Damage() {
        return stats.Damage;
    }*/

    void Awake() {
        //health = gameObject.AddComponent<Health>();
    }

    //public abstract void OnMouseDown();

    private void Start() {
        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);
        actionPoints = stats.ActionPoints;

        Debug.Log($"Unit Name: {stats.Name}, Max Health: {stats.MaxHealth}, Action Points: {stats.ActionPoints}");
    }

    public void PerformMove(string moveType, Lifeforms target) {
        StartCoroutine(PerformMoveType(moveType, target));
    }

    public IEnumerator PerformMoveType(string moveType, Lifeforms target) {
        Debug.Log($"{target.stats.Name} is the target");
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
    }

    /*public void PerformMove() {
        specialMove.Execute(this);
    }*/

    public void PerformUltimateMove() {
        ultimateMove.Execute(this);
    }
}

// Serialization probably not needed
[System.Serializable]
public class Stats {
    [SerializeField] string name;
    [SerializeField] private float maxHealth;
    [SerializeField] private float actionPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float defence;

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public float MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float ActionPoints {
        get { return actionPoints; }
        set { actionPoints = value; }
    }

    public float MoveSpeed {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float Defence {
        get { return defence; }
        set { defence = value; }
    }

}