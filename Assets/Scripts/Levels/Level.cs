using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level")]
public class Level : ScriptableObject {
    [SerializeField] private string levelName;

    [SerializeField] private List<GameObject> playerUnits;

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
    }
}
