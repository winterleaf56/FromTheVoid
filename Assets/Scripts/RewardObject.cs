using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardObject : MonoBehaviour {

    [SerializeField] private TMP_Text rewardValueTxt;
    [SerializeField] private Image rewardImg;

    public void SetRewardData(RewardData data) {
        if (data.rewardType == RewardType.Coins || data.rewardType == RewardType.VoidShards) {
            rewardValueTxt.SetText(data.amount.ToString());
        } else if (data.rewardType == RewardType.Item) {
            rewardValueTxt.SetText("No data at this time"); // Change this to the name of the item later
        } else if (data.rewardType == RewardType.Unit) {
            rewardValueTxt.SetText(data.unitReference.GetComponent<Friendly>().UnitStats.name);
        }
        rewardImg.sprite = RewardIconDatabase.Instance.GetIcon(data.rewardType);
    }

}
