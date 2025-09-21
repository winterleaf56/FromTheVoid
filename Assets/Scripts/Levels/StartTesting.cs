using System.Collections.Generic;
using UnityEngine;

public class StartTesting : MonoBehaviour {

    [SerializeField] private List<Friendly> unitsToTest;
    [SerializeField] private Level testingLevel;

    public Level TestingLevel => testingLevel;

    public static StartTesting Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTestUnits()
    {
        /*for (int i = 0; i < unitsToTest.Count; i++) {
            BattleManager.SelectedLevel.PlayerUnits.Add(unitsToTest[i]);
        }*/

        foreach (Friendly unit in unitsToTest)
        {
            Debug.Log($"Added {unit.UnitStats.UnitName} to BattleManager.SelectedLevel.PlayerUnits");
            //BattleManager.SelectedLevel.PlayerUnits.Add(unit);
            GameManager.Instance.SelectedLevel.AddPlayerUnit(unit);
        }

        GameManager.Instance.StartTestingLevel();
    }
}
