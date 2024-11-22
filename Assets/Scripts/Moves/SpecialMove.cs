using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Special Attack")]
public class SpecialMove : BasicMove {
    [SerializeField] private float cooldown;
}
