using UnityEngine;

public abstract class Lifeforms : MonoBehaviour, IDamageable {
    public Health health;

    public float damage;

    public float actionPoints;

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
}
