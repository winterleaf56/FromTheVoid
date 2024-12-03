using UnityEngine;

public class LevelBase : ScriptableObject {
    [SerializeField] private string levelName;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] enemySpawns;
    [SerializeField] private GameObject[] friendlySpawns;



}
