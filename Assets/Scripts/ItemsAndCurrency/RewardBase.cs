using UnityEngine;
using UnityEngine.Events;

public abstract class RewardBase : ScriptableObject {
    [SerializeField] protected int value;

    /*private UnityEvent distributeReward;

    private void Awake() {
        distributeReward.AddListener(DistributeReward);
    }*/

    public virtual void DistributeReward() {

    }
}
