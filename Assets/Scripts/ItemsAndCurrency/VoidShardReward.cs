using UnityEngine;

[CreateAssetMenu(fileName = "VoidShardReward", menuName = "Rewards/Void Shard Reward")]
public class VoidShardReward : RewardBase {

    public override void DistributeReward() {
        PlayerDetailsManager.Instance.AddVoidShards(value);
    }

}
