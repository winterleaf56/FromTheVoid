using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Basic Attack")]
public class BasicMove : ActionBase {
    [SerializeField] private float baseDamage;

    public virtual float CalculateDamage(Friendly unit) {
        return baseDamage;
    }

    public override void Execute(Friendly unit) {
        Debug.Log("Performing move: " + moveName);
        float damage = CalculateDamage(unit);
    }
}