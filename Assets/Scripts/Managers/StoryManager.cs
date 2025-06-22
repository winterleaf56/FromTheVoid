using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour {
    [SerializeField] private List<StoryStep> storySteps;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    private int currentStepIndex = 0;

    [SerializeField] private GameObject closeBtn;
    [SerializeField] private GameObject nextPageBtn;
    [SerializeField] private GameObject StoryUI;

    private void Start() {
        storySteps = BattleManager.SelectedLevel.StorySteps;
        Debug.LogAssertion($"Level Name: {BattleManager.SelectedLevel.LevelName}");
        Debug.Log($"Story steps loaded: {storySteps.Count}");
        ShowNextStep();
    }

    public void ShowNextStep() {
        if (currentStepIndex < storySteps.Count) {
            titleText.text = storySteps[currentStepIndex].titleText;
            bodyText.text = storySteps[currentStepIndex].bodyText;
            currentStepIndex++;

            if (currentStepIndex == storySteps.Count) {
                FinalStep();
            }
        } else {
            Debug.Log("No more steps to show.");
        }
    }

    private void FinalStep() {
        Debug.Log("Story complete!");

        closeBtn.SetActive(true);
        nextPageBtn.SetActive(false);
    }
}
