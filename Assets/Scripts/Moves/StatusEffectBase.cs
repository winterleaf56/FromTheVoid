using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectBase", menuName = "Status Effect/Base")]
public abstract class StatusEffectBase : ScriptableObject {
    public GameObject effectPrefab;
    private string effectName;
    private int effectDuration;
    private int effectValue;
    //[SerializeField] private int effectType;

    public string EffectName => effectName;
    public float EffectDuration => effectDuration;
    public int EffectValue => effectValue;

    public virtual void Initialize(GameObject effectPrefab, string effectName, int duration, int value) {
        this.effectPrefab = effectPrefab;
        this.effectName = effectName;
        effectDuration = duration;
        effectValue = value;
    }

    public abstract void ApplyEffect(Lifeforms target);

    public abstract void RemoveEffect(Lifeforms target);

    /*private virtual void SetEffects(Lifeforms unit) {
        UIManager.SetEffects(effectPrefab, effectDuration, effectValue);
    }*/
}



[CreateAssetMenu(fileName = "ActionPointEffect", menuName = "Status Effect/Action Point Effect")]
public class ActionPointEffect : StatusEffectBase {
    public override void Initialize(GameObject effectPrefab, string effectName, int duration, int value) {
        base.Initialize(effectPrefab, effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.AddActionPointRecovery(EffectValue);
        Debug.Log("Applying Action Point Effect");
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.AddActionPointRecovery(-EffectValue);
        foreach (var effect in target.GetStatusEffects()) {
            if (effect.GetData().EffectName == EffectName) {
                target.GetStatusEffects().Remove(effect);
                break;
            }
        }
    }
}


/*[CreateAssetMenu(fileName = "StaminaStimEffect", menuName = "Status Effect/Stamina Stim Effect")]
public class StaminaStimEffect : StatusEffectBase {
    public override void Initialize(string effectName, int duration, int value) {
        base.Initialize(effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target;
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.SetActionPointRecovery(0);
    }
}*/

[CreateAssetMenu(fileName = "BandageEffect", menuName = "Status Effect/Bandage Effect")]
public class BandageEffect : StatusEffectBase {
    public override void Initialize(GameObject effectPrefab, string effectName, int duration, int value) {
        base.Initialize(effectPrefab, effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.SetHealOverTime(EffectValue);
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.SetHealOverTime(-EffectValue);
    }
}

[CreateAssetMenu(fileName = "DefenceEffect", menuName = "Status Effect/Brace Effect")]
public class DefenceEffect : StatusEffectBase {
    public override void Initialize(GameObject effectPrebab, string effectName, int duration, int value) {
        base.Initialize(effectPrefab, effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.SetDefence(EffectValue);
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.SetDefence(0);
    }
}

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Status Effect/Damage Effect")]
public class DamageEffect : StatusEffectBase {
    public override void Initialize(GameObject effectPrebab, string effectName, int duration, int value) {
        base.Initialize(effectPrefab, effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.SetDamageOverTime(EffectValue);
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.SetDamageOverTime(-EffectValue);
    }
}

public class AsleepEffect : StatusEffectBase {
    public override void Initialize(GameObject effectPrefab, string effectName, int duration, int value) {
        base.Initialize(effectPrefab, effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        Debug.Log("Asleep");
    }

    public override void RemoveEffect(Lifeforms target) {
        Debug.Log("Awake");
    }
}