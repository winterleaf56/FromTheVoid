using UnityEngine;

public abstract class ObjectiveBase : ScriptableObject {
    public string objectiveText;
    public bool isCompleted = false;

    private void OnEnable() {
        isCompleted = false;
    }

    public virtual void CheckObjective() {

    }
}
