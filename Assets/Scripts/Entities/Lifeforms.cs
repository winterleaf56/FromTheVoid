using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    [SerializeField] protected BasicMove basicMove;
    [SerializeField] protected SpecialMove specialMove;
    [SerializeField] protected UltimateMove ultimateMove;

    public Health health;

    public float damage;

    private float actionPoints;

    public abstract void Attack();

    public abstract void Die();

    public abstract void Damage(float value);

    public float Damage() {
        return damage;
    }

    void Awake() {
        //health = gameObject.AddComponent<Health>();
    }

    //public abstract void OnMouseDown();

    public void PerformBasicMove() {
        basicMove.Execute(this);
        Debug.Log($"Performing basic move: {basicMove.name}");
    }

    public void PerformMove() {
        specialMove.Execute(this);
    }

    public void PerformUltimateMove() {
        ultimateMove.Execute(this);
    }
}
