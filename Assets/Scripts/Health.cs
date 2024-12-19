using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public bool isDead { get; private set; }

    public Action<float> OnHealthChanged;

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

    public void TakeDamage(float value) {
        Debug.Log($"Taking {value} damage");
        if (isDead) return;

        health -= value;

        if (health <= 0) {
            health = 0;
            isDead = true;
            OnDeath();
        }

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
    }

    void OnHealthUpdate(float value) {
        Debug.Log($"Health changed to {value}");
    }
}
