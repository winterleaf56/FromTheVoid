using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectBase", menuName = "Status Effect/Base")]
public abstract class StatusEffectBase : ScriptableObject {
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private string effectName;
    [SerializeField] private int effectDuration;
    [SerializeField] private int effectValue;
    //[SerializeField] private int effectType;

    public string EffectName => effectName;
    public float EffectDuration => effectDuration;
    public int EffectValue => effectValue;

    public virtual void Initialize(string effectName, int duration, int value) {
        this.effectName = effectName;
        effectDuration = duration;
        effectValue = value;
    }

    public abstract void ApplyEffect(Lifeforms target);

    public abstract void RemoveEffect(Lifeforms target);
}



[CreateAssetMenu(fileName = "ActionPointEffect", menuName = "Status Effect/Action Point Effect")]
public class ActionPointEffect : StatusEffectBase {
    public override void Initialize(string effectName, int duration, int value) {
        base.Initialize(effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.AddActionPointRecovery(EffectValue);
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.AddActionPointRecovery(-EffectValue);
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
    public override void Initialize(string effectName, int duration, int value) {
        base.Initialize(effectName, duration, value);
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
    public override void Initialize(string effectName, int duration, int value) {
        base.Initialize(effectName, duration, value);
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
    public override void Initialize(string effectName, int duration, int value) {
        base.Initialize(effectName, duration, value);
    }

    public override void ApplyEffect(Lifeforms target) {
        target.stats.SetDamageOverTime(EffectValue);
    }

    public override void RemoveEffect(Lifeforms target) {
        target.stats.SetDamageOverTime(-EffectValue);
    }
}