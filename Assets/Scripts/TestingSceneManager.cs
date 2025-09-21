using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TestingSceneManager : MonoBehaviour {
    [SerializeField] private Level testingLevel;

    [SerializeField] private List<Friendly> unitsToTest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Awake() {
        BattleManager.SelectedLevel = testingLevel;
        Debug.Log($"TestingLevel environment: {testingLevel.EnvironmentPrefab.name}");
        
        for (int i = 0; i < unitsToTest.Count; i++) {
            BattleManager.SelectedLevel.PlayerUnits.Add(unitsToTest[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        GameObject.Find("BattleManger").SetActive(true);
    }*/

    /*public static TestingSceneManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadTestUnits() {
        *//*for (int i = 0; i < unitsToTest.Count; i++) {
            BattleManager.SelectedLevel.PlayerUnits.Add(unitsToTest[i]);
        }*//*

        foreach (Friendly unit in BattleManager.SelectedLevel.PlayerUnits) {
            Debug.Log($"Added {unit.UnitStats.UnitName} to BattleManager.SelectedLevel.PlayerUnits");
            BattleManager.SelectedLevel.PlayerUnits.Add(unit);
        }

        GameManager.Instance.StartTestingLevel();
    }*/
}
