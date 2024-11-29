using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Special Attack")]
public class SpecialMove : ActionBase {
    [SerializeField] private float cooldown;
}
