using UnityEngine;

public abstract class ObjectiveBase : ScriptableObject {
    public string objectiveText;
    public bool isCompleted;


    public virtual void CheckObjective() {

    }
}
