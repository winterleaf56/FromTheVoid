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

    // Both of these aren't being used here anymore but I'll keep them here for now
    //[SerializeField] private GameObject levelButtons;
    //[SerializeField] private Level tutorial;

    private int selectedUnits;

    // This will be for disabling the buttons so you can only have 4 selected at once
    // Change this so that instead of 4 units, it is the number of units required for the level
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

    private void OnDestroy() {
        unitSelected -= SelectedUnit;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        /*if (tutorial.LevelCompleted == true) {
            levelButtons.transform.Find("CompletedImage").gameObject.SetActive(true);
        }*/

        PlayerDetailsManager playerDetails = PlayerDetailsManager.Instance;
        MenuUIManager menuUIManager = MenuUIManager.Instance;

        //menuUIManager.UnitSelectPanel.SetActive(true);

        if (playerDetails != null) {
            MenuUIManager.updateUserDetails?.Invoke();
            if (playerDetails.PlayerName == null || playerDetails.PlayerName == "") {
                // Open the name input UI
                menuUIManager.UsernamePanel.SetActive(true);
                menuUIManager.SubmitUsernameBtn.onClick.AddListener(() => {
                    string playerName = menuUIManager.UsernameInput.text;
                    if (!string.IsNullOrEmpty(playerName)) {
                        playerDetails.SetPlayerName(playerName);
                        menuUIManager.UsernamePanel.SetActive(false);
                        Debug.Log($"Player name set to: {playerName}");
                        MenuUIManager.updateUserDetails?.Invoke();
                        SaveManager.Instance.SavePlayerData();
                    } else {
                        Debug.LogWarning("Player name cannot be empty.");
                    }
                });

            }
        }

        //SaveManager.Instance.LoadGame?.Invoke();
    }

    public void StartLevel() {
        BattleManager.SelectedLevel = selectedLevel;
        SceneManager.LoadScene("Tutorial");
        //SceneManager.LoadScene(selectedLevel.name);
    }

    private void SelectedUnit(int value) {
        selectedUnits += value;

        // Message here as well. Change this so instead of 4 units, it is the number of units required for the level
        /*if (selectedUnits == 4) {
            startButton.GetComponent<Button>().interactable = true;
        } else {
            startButton.GetComponent<Button>().interactable = false;
        }*/

        if (selectedUnits == selectedLevel.RequiredNumberOfUnits) {
            startButton.GetComponent<Button>().interactable = true;
        } else {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void SetSelectedLevel(Level level) {
        selectedLevel = level;
    }

    public int RandomNumber() {
        return 25;
    }

    public void LoadUnitButtons() {
        foreach (Transform child in unitButtonContainer.transform) {
            Destroy(child.gameObject);
        }

        foreach (GameObject unit in playerUnits) {
            GameObject button = Instantiate(buttonPrefab, unitButtonContainer.transform);
            button.GetComponentInChildren<TMP_Text>().SetText(unit.GetComponent<Friendly>().UnitStats.UnitName);

            button.GetComponent<UnitButton>().SetUnit(unit);
        }
    }

    // Add Start Mission which adds the units the player selects to the array in BattleManager
}
