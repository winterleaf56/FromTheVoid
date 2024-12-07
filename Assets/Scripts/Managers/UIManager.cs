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

        /*GameObject button = Instantiate(testButton, actionButtonsParent.transform);
        button.GetComponent<Button>().onClick.AddListener(TestingForThis);*/

        float buttonSpacing = 10f; // Adjust as needed

        // Action buttons
        ActionBase[] actions = unit.GetActions();
        float currentXPosition = 0; // Start from the rightmost side (local space)

        for (int i = actions.Length - 1; i >= 0; i--) { // Reverse loop for leftward positioning
            ActionBase action = actions[i];
            if (action.buttonPrefab != null) {
                GameObject button = Instantiate(action.buttonPrefab, actionButtonsParent);
                //button.gameObject.GetComponent<Button>().onClick.AddListener(TestingForThis);
                
                //button.GetComponent<Button>().onClick.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
                button.GetComponent<Button>().onClick.AddListener(TestingForThis);
                Debug.Log("Button Setup");
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                float buttonWidth = buttonRect.rect.width;

                // Calculate position: Start from 0 and move leftward
                buttonRect.anchoredPosition = new Vector2(currentXPosition, 0);
                currentXPosition -= (buttonWidth + buttonSpacing); // Move left
            }
        }

        // Move buttons
        ActionBase[] moves = unit.GetMoves();
        currentXPosition = 0; // Reset for move buttons

        for (int i = moves.Length - 1; i >= 0; i--) { // Reverse loop for leftward positioning
            ActionBase move = moves[i];
            if (move.buttonPrefab != null) {
                GameObject button = Instantiate(move.buttonPrefab, moveButtonsParent);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                float buttonWidth = buttonRect.rect.width;

                // Calculate position: Start from 0 and move leftward
                buttonRect.anchoredPosition = new Vector2(currentXPosition, 0);
                currentXPosition -= (buttonWidth + buttonSpacing); // Move left
            }
        }
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
