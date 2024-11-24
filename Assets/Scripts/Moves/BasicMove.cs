using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Moves/Basic Attack")]
public class BasicMove : ActionBase {
    [SerializeField] private float baseDamage;

    public virtual float CalculateDamage() {
        return baseDamage;
    }

    public override void Execute(Lifeforms unit) {
        Debug.Log("Performing move: " + moveName);
        //float damage = CalculateDamage(unit);
    }

    public override void Execute() {
        Debug.Log("Performing move: " + moveName);
        float damage = CalculateDamage();
    }
}