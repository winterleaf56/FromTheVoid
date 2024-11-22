using UnityEngine;

public class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] private float actionPoints;

    public virtual void Execute(Friendly unit) {
        Debug.Log("Performing move: " + moveName);
    }
}
