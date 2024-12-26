using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerUnits;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject unitButtonContainer;

    [SerializeField] private Level selectedLevel;

    [SerializeField] private GameObject startButton;

    [SerializeField] private GameObject levelButtons;
    [SerializeField] private Level tutorial;

    private int selectedUnits;

    // This will be for disabling the buttons so you can only have 4 selected at once
    public static Action<int> unitSelected;

    public GameObject[] PlayerUnits => playerUnits;

    public Level SelectedLevel => selectedLevel;

    public static GameManager Instance;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        unitSelected += SelectedUnit;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (tutorial.LevelCompleted == true) {
            levelButtons.transform.Find("CompletedImage").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel() {
        BattleManager.SelectedLevel = selectedLevel;
        SceneManager.LoadScene(selectedLevel.name);
    }

    private void SelectedUnit(int value) {
        selectedUnits += value;

        if (selectedUnits == 4) {
            startButton.GetComponent<Button>().interactable = true;
        } else {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void SetSelectedLevel(Level level) {
        selectedLevel = level;
    }

    public void LoadUnitButtons() {
        foreach (Transform child in unitButtonContainer.transform) {
            Destroy(child.gameObject);
        }

        foreach (GameObject unit in playerUnits) {
            GameObject button = Instantiate(buttonPrefab, unitButtonContainer.transform);
            button.GetComponentInChildren<TMP_Text>().SetText(unit.name);

            button.GetComponent<UnitButton>().SetUnit(unit);
        }
    }

    // Add Start Mission which adds the units the player selects to the array in BattleManager
}
