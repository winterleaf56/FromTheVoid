using UnityEngine;

public static class StatusEffectFactory {
    public static ActionPointEffect CreateActionPointEffect(GameObject effectPrefab, string effectName, int duration, int damageAmount) {
        ActionPointEffect effect = ScriptableObject.CreateInstance<ActionPointEffect>();
        effect.Initialize(effectPrefab, effectName, duration, damageAmount);
        return effect;
    }

    public static DamageEffect CreateDamageEffect(GameObject effectPrefab, string effectName, int duration, int damageAmount) {
        DamageEffect effect = ScriptableObject.CreateInstance<DamageEffect>();
        effect.Initialize(effectPrefab, effectName, duration, damageAmount);
        return effect;
    }
}
