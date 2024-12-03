using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] protected int actionPoints;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;

    protected bool isAOE;

    public virtual IEnumerator Execute(Lifeforms unit, Lifeforms target) {
        Debug.Log("Performing move: " + moveName + ", against: " + target);
        yield return null;
    }

    public virtual IEnumerator Execute(Lifeforms unit) {
        Debug.Log("Performing move: " + moveName);
        yield return null;
    }

    public int GetAPRequirement() {
        return actionPoints;
    }

    /*public virtual void Execute() {
        Debug.Log("Performing move: " + moveName);
    }*/
}
