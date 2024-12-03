using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level")]
public class Level : ScriptableObject {
    [SerializeField] private string levelName;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] enemySpawns;
    [SerializeField] private GameObject[] friendlySpawns;

    [SerializeField] private GameObject environmentPrefab;
}
