using System.Collections.Generic;
using UnityEngine;

public enum RewardType {
    VoidShards,
    Coins,
    Item
}

[CreateAssetMenu(fileName = "CustomReward", menuName = "Reward")]
public class CustomReward : ScriptableObject {
    [SerializeField]
    private List<RewardData> rewards = new List<RewardData>();

    public List<RewardData> Rewards => rewards;

    public Sprite GetRewardIcon(RewardType type) {
        return RewardIconDatabase.Instance?.GetIcon(type);
    }

    public void DistributeRewards() {
        foreach (var reward in rewards) {
            switch (reward.rewardType)  {
                case RewardType.VoidShards:
                    PlayerDetailsManager.Instance.AddVoidShards(reward.amount);
                    Debug.Log($"Void Shards Rewarded: {reward.amount}");
                    break;
                case RewardType.Coins:
                    PlayerDetailsManager.Instance.AddCoins(reward.amount);
                    Debug.Log($"Coins Rewarded: {reward.amount}");
                    break;
                case RewardType.Item:
                    if (reward.itemReference != null) {
                        //InventoryManager.Instance.AddItem(reward.itemReference);
                        Debug.Log($"Item Rewarded");
                    }
                    break;
            }
        }
    }
}

[System.Serializable]
public class RewardData {
    public RewardType rewardType;
    public int amount;                // Used if rewardType is VoidShards or Coins
    public GameObject itemReference;  // Used if rewardType is Item
}
