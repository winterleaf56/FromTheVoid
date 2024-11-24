using UnityEngine;

public class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] private float actionPoints;

    public virtual void Execute(Lifeforms unit) {
        Debug.Log("Performing move: " + moveName);
    }

    public virtual void Execute() {
        Debug.Log("Performing move: " + moveName);
    }
}
