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
    [SerializeField] private GameObject confirmPage;
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private GameObject cancelBtn;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TMP_Text costTxt;


    [SerializeField] private float buttonSpacing = 100;

    public UnityEvent MoveComplete;

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
        GameObject newButton;

        foreach (var action in actions) {
            newButton = Instantiate(buttonPrefab, parent);
            confirmBtn.GetComponent<Button>().interactable = false;
            action.SetupButton(newButton.GetComponent<Button>(), unit, confirmPage, confirmBtn, cancelBtn.GetComponent<Button>());
        }

        newButton = Instantiate(backButton, parent);
        newButton.GetComponent<Button>().onClick.AddListener(() => parent.parent.gameObject.SetActive(false));

    }

    private void ResetUI() {

    }






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
