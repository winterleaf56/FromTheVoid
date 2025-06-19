using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private TutorialStep[] tutorialSteps;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    private int currentStepIndex = 0;

    [SerializeField] private GameObject closeBtn;
    [SerializeField] private GameObject nextPageBtn;
    [SerializeField] private GameObject tutorialUI;

    private void Start() {
        ShowNextStep();
    }

    public void ShowNextStep() {
        if (currentStepIndex < tutorialSteps.Length) {
            titleText.text = tutorialSteps[currentStepIndex].titleText;
            bodyText.text = tutorialSteps[currentStepIndex].bodyText;
            currentStepIndex++;

            if (currentStepIndex == tutorialSteps.Length) {
                FinalStep();
            }
        } else {
            Debug.Log("No more steps to show.");
        }
    }

    private void FinalStep() {
        Debug.Log("Tutorial complete!");

        closeBtn.SetActive(true);
        nextPageBtn.SetActive(false);
    }
}
