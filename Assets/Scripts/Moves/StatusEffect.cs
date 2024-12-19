using UnityEngine;

public class StatusEffect {
    private StatusEffectBase data;
    private float remainingDuration;
    private Lifeforms target;

    public StatusEffect(StatusEffectBase data, Lifeforms target, float customDuration = -1) {
        this.data = data;
        this.target = target;
        this.remainingDuration = customDuration > 0 ? customDuration : data.EffectDuration;
    }

    public void ApplyEffect() {
        data.ApplyEffect(target);
    }

    public void UpdateEffect() {
        if (remainingDuration > 0) {
            remainingDuration--;
        }
    }

    public bool IsExpired() {
        return remainingDuration <= 0;
    }
}
