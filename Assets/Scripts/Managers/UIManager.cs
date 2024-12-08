using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private TMP_Text selectedUnitName;
    [SerializeField] private TMP_Text selectedUnitHealthTxt;
    [SerializeField] private TMP_Text selectedUnitAPTxt;

    [Header("Move Buttons")]
    [SerializeField] private Transform moveButtonsParent;
    [SerializeField] private GameObject moveBackButton;

    [Header("Action Buttons")]
    [SerializeField] private Transform actionButtonsParent;
    [SerializeField] private GameObject actionBackButton;

    [Header("Confirmation UI")]
    [SerializeField] private GameObject confirmPage; // Confirmation page UI element
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private GameObject cancelBtn;


    [SerializeField] private float buttonSpacing = 100;


    public static UIManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateStatBar(string name, float health, int actionPoints) {
        selectedUnitName.SetText(name);
        selectedUnitHealthTxt.SetText($"Health: {health.ToString()}");
        selectedUnitAPTxt.SetText($"Action Points: {actionPoints.ToString()}");
    }

    public void UpdateStatBar(string name, float health) {
        selectedUnitName.SetText(name);
        selectedUnitHealthTxt.SetText($"Health: {health.ToString()}");
        selectedUnitAPTxt.SetText("");
    }

    public void ToggleStats(bool enable) {
        selectedUnitName.gameObject.SetActive(enable);
        selectedUnitHealthTxt.gameObject.SetActive(enable);
        selectedUnitAPTxt.gameObject.SetActive(enable);
    }

    public void TestingForThis() {
        Debug.Log("HELLO IS THIS WORKING");
    }

    [SerializeField] GameObject testButton;

    public void LoadButtons(Lifeforms unit) {
        // Clear previous buttons
        foreach (Transform child in moveButtonsParent) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in actionButtonsParent) {
            Destroy(child.gameObject);
        }

        float buttonSpacing = 10f; // Adjust as needed

        // Action buttons
        SetButtons(unit.GetActions(), unit, actionButtonsParent, buttonSpacing, actionBackButton);

        // Move buttons
        SetButtons(unit.GetMoves(), unit, moveButtonsParent, buttonSpacing, moveBackButton);
    }

    private void SetButtons(ActionBase[] actions, Lifeforms unit, Transform parent, float buttonSpacing, GameObject backButton) {
        RectTransform parentRect = parent.GetComponent<RectTransform>();
        RectTransform buttonRect;
        GameObject newButton;
        float buttonWidth;

        // Start from the rightmost edge of the parent
        float currentXPosition = parentRect.rect.width / 2f - 10; // Start at the right edge relative to the parent's center

        foreach (var action in actions) {
            // Instantiate the button as a child of the parent
            newButton = Instantiate(action.buttonPrefab, parent);

            // Get the button's RectTransform
            buttonRect = newButton.GetComponent<RectTransform>();

            // Get button width
            buttonWidth = buttonRect.rect.width;

            // Calculate and apply the position
            currentXPosition -= (buttonWidth / 2f); // Move left by half the button width
            buttonRect.anchoredPosition = new Vector2(currentXPosition, 0);

            // Adjust for the next button's position
            currentXPosition -= (buttonWidth / 2f + buttonSpacing);

            action.SetupButton(newButton.GetComponent<Button>(), unit, confirmPage, confirmBtn, cancelBtn.GetComponent<Button>());

            //newButton.GetComponent<Lifeforms>().SetupButton(newButton.GetComponent<Button>(), newButton.GetComponent<Lifeforms>(), confirmPage, confirmBtn);
        }

        newButton = Instantiate(backButton, parent);
        buttonRect = newButton.GetComponent<RectTransform>();
        buttonWidth = buttonRect.rect.width;
        currentXPosition -= (buttonWidth / 2f);
        buttonRect.anchoredPosition = new Vector2(currentXPosition, 0);
        currentXPosition -= (buttonWidth / 2f + buttonSpacing);

        newButton.GetComponent<Button>().onClick.AddListener(() => parent.gameObject.SetActive(false));

    }








    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
