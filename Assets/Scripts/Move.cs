using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Move")]
public class Move : ScriptableObject {
    public string moveName;
    public float baseDamage;
    public float actionPoints;
}
