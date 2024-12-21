using UnityEngine;

[System.Serializable]
public class StatusEffect {
    private StatusEffectBase data;
    public GameObject effectPrefab;
    private float remainingDuration;
    private Lifeforms target;

    public StatusEffect(StatusEffectBase data, Lifeforms target, float customDuration = -1) {
        this.data = data;
        this.effectPrefab = data.effectPrefab;
        this.target = target;
        this.remainingDuration = customDuration > 0 ? customDuration : data.EffectDuration;
        Debug.Log("Remaining time on effect: " + remainingDuration);
    }

    public StatusEffectBase GetData() {
        return data;
    }

    public Lifeforms GetTarget() {
        return target;
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
