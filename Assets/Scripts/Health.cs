using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public bool isDead { get; private set; }

    public Action<float> OnHealthChanged;
    public Action<Lifeforms> onAttacked;

    private void Start() {
        OnHealthChanged += OnHealthUpdate;

        health = maxHealth;
        OnHealthChanged(maxHealth);
    }

    public void InitializeHealth(float maxHealth, float currentHealth) {
        this.health = currentHealth;
        this.maxHealth = maxHealth;
    }

    public float GetHealth() {
        return health;
    }

    public void TakeDamage(float value, Lifeforms attacker) {
        Debug.Log($"Taking {value} damage");
        if (isDead) return;

        health -= value;

        if (health <= 0) {
            health = 0;
            isDead = true;
            OnDeath();
            return;
        }

        //onAttacked(attacker);

        //OnHealthChanged(health);
    }

    public void AddHealth(float value) {
        if (isDead) return;

        health += value;

        if (health > maxHealth) {
            health = maxHealth;
        }

        OnHealthChanged(health);
    }

    void OnDeath() {
        Debug.Log("Unit Dead");
        BattleManager.unitDied(transform.GetComponent<Lifeforms>());
        transform.gameObject.SetActive(false);
    }

    void OnHealthUpdate(float value) {
        Debug.Log($"Health changed to {value}");
    }
}
