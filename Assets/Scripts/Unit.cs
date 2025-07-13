using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Units/Unit")]
public class Unit : ScriptableObject {
    [SerializeField] private string unitName;
    [SerializeField] private int actionPoints;
    [SerializeField] private int actionPointRecovery;
    [SerializeField] private int ultimatePoints;
    [SerializeField] private int maxUltimatePoints;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMoveDistance;
    [SerializeField] private float defence;
    [SerializeField] private int duplicates;

    public string UnitName => unitName;
    public int ActionPoints => actionPoints;
    public int ActionPointRecovery => actionPointRecovery;
    public int UltimatePoints => ultimatePoints;
    public int MaxUltimatePoints => maxUltimatePoints;
    public float MaxHealth => maxHealth;
    public float MaxMoveDistance => maxMoveDistance;
    public float Defence => defence;
    public int Duplicates => duplicates;


    private void Start() {

    }
}
