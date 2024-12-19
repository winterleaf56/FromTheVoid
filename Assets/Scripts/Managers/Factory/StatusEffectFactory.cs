using UnityEngine;

public static class StatusEffectFactory {
    public static ActionPointEffect CreateActionPointEffect(string effectName, int duration, int damageAmount) {
        ActionPointEffect effect = ScriptableObject.CreateInstance<ActionPointEffect>();
        effect.Initialize(effectName, duration, damageAmount);
        return effect;
    }

    public static DamageEffect CreateDamageEffect(string effectName, int duration, int damageAmount) {
        DamageEffect effect = ScriptableObject.CreateInstance<DamageEffect>();
        effect.Initialize(effectName, duration, damageAmount);
        return effect;
    }
}
