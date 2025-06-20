using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level")]
public class Level : ScriptableObject {
    [SerializeField] private string levelName;

    [SerializeField] private GameObject environmentPrefab;
    public GameObject EnvironmentPrefab => environmentPrefab;

    [SerializeField] private List<GameObject> playerUnits;

    [SerializeField] private List<StoryStep> storySteps;
    public List<StoryStep> StorySteps => storySteps;

    [SerializeField] private List<ObjectiveBase> objectives;
    public List<ObjectiveBase> Objectives => objectives;

    [SerializeField] private int reward;
    [SerializeField] private int requiredNumberOfUnits;

    [SerializeField] private bool levelCompleted;

    public string LevelName => levelName;
    public bool LevelCompleted => levelCompleted;

    public List<GameObject> PlayerUnits => playerUnits;

    public int Reward => reward;
    public int RequiredNumberOfUnits => requiredNumberOfUnits;

    private void OnEnable() {
        ClearUnits();
    }

    public void AddPlayerUnit(GameObject unit) {
        playerUnits.Add(unit);
        Debug.Log($"Added {unit.name}");
    }

    public void RemovePlayerUnit(GameObject unit) {
        playerUnits.Remove(unit);
        Debug.Log($"Removed {unit.name}");
    }

    public void ClearUnits() {
        playerUnits.Clear();
    }

    public void PrintUnitsSelected() {
        foreach (GameObject unit in playerUnits) {
            Debug.Log(unit.name);
        }
    }

    public void CompleteLevel() {
        levelCompleted = true;

        Debug.Log($"Level {levelName} completed: {levelCompleted}. Invoke Saving...");
        SaveManager.Instance.SaveGame.Invoke();
        //SaveManager.Instance.SaveLevelCompleted(levelName, levelCompleted);
        //SaveLevelCompleted();
    }

    /*public void LoadCompletionStatus() {
        levelCompleted = true;
    }*/

    public void ChangeCompletionStatus(bool status) {
        levelCompleted = status;
        SaveManager.Instance.SaveGame?.Invoke();
    }

    /*public void SaveLevelCompleted() {
        SaveManager.Instance.SaveLevelCompleted(levelName, levelCompleted);
        //GameManager.Instance.TutorialComplete();
    }

    public void LoadLevelCompleted() {
        if (SaveManager.Instance == null) {
            Debug.LogError("SaveManager instance is not available.");
            return;
        }
        levelCompleted = SaveManager.Instance.LoadLevelCompleted(levelName);
        Debug.Log($"Level {levelName} completed status: {levelCompleted}");

        if (levelCompleted) {
            GameManager.Instance.TutorialComplete();
        }
    }*/
}
