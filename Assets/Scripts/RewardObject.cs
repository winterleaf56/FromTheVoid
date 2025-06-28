using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardObject : MonoBehaviour {

    [SerializeField] private TMP_Text rewardValueTxt;
    [SerializeField] private Image rewardImg;

    public void SetRewardData(RewardData data) {
        rewardValueTxt.SetText(data.amount.ToString());

        rewardImg.sprite = RewardIconDatabase.Instance.GetIcon(data.rewardType);
    }

}
