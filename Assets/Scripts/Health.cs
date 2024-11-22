using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public bool isDead { get; private set; }

    public Action<float> OnHealthChanged;

    private void Start() {
        health = maxHealth;
        OnHealthChanged(maxHealth);
    }

    public void GetHealth() {

    }

    public void TakeDamage(float value) {
        if (isDead) return;

        health -= value;

        if (health <= 0) {
            health = 0;
            isDead = true;
        }

        OnHealthChanged(health);
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

    }
}
