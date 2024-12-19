using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StatusEffectManager : MonoBehaviour {
    public static StatusEffectManager Instance { get; private set; }

    public enum StatusEffectType {
        ActionPointEffect,
        BandageEffect,
        DefenceEffect,
        DamageEffect
    }

    private List<StatusEffect> friendlyActiveEffects = new List<StatusEffect>();
    private List<StatusEffect> enemyActiveEffects = new List<StatusEffect>();
    private Dictionary<StatusEffectType, Func<string, int, int, StatusEffectBase>> effectFactories;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        effectFactories = new Dictionary<StatusEffectType, Func<string, int, int, StatusEffectBase>> {
            { StatusEffectType.ActionPointEffect, StatusEffectFactory.CreateActionPointEffect },
            { StatusEffectType.DamageEffect, StatusEffectFactory.CreateDamageEffect }
        };
    }

    /*public void AddEffect(StatusEffectBase effectData, Lifeforms target, float customDuration = -1) {
        var newEffect = new StatusEffect(effectData, target, customDuration);
        activeEffects.Add(newEffect);
        newEffect.ApplyEffect();
    }*/

    public void AddEffect(StatusEffectType effectType, string effectName, int duration, int value, Lifeforms target) {
        // Use the dictionary to create the effect
        if (effectFactories.TryGetValue(effectType, out var factory)) {
            StatusEffectBase effectData = factory(effectName, duration, value);
            if (effectData != null) {
                var newEffect = new StatusEffect(effectData, target);

                if (target.GetComponent<Friendly>()) {
                    friendlyActiveEffects.Add(newEffect);
                } else if (target.GetComponent<Enemy>()) {
                    enemyActiveEffects.Add(newEffect);
                } else {
                    Debug.LogError($"Target {target.name} is not a Friendly or Enemy.");
                }

                newEffect.ApplyEffect();
            }
        } else {
            Debug.LogError($"Effect type {effectType} not found.");
        }
    }

    public void UpdateFriendlyEffects() {
        for (int i = friendlyActiveEffects.Count - 1; i >= 0; i--) {
            friendlyActiveEffects[i].UpdateEffect();
            if (friendlyActiveEffects[i].IsExpired()) {
                friendlyActiveEffects.RemoveAt(i);
            }
        }
        
    }

    public void UpdateEnemyEffects() {
        for (int i = enemyActiveEffects.Count - 1; i >= 0; i--) {
            enemyActiveEffects[i].UpdateEffect();
            if (enemyActiveEffects[i].IsExpired()) {
                enemyActiveEffects.RemoveAt(i);
            }
        }
    }

    public void RemoveAllEffects() {
        friendlyActiveEffects.Clear();
        enemyActiveEffects.Clear();
    }

    /*public void UpdateEffects() {
        foreach (var effect in activeEffects) {
            effect.ApplyEffect(target);
            effect.remainingDuration -= 1;
            if (effect.remainingDuration <= 0) {
                Debug.Log($"{effect.data.effectName} expired on {target.name}");
                expiredEffects.Add(effect);
            }
        }

        // Remove expired effects
        activeEffects.RemoveAll(e => expiredEffects.Contains(e));
    }*/
}
