using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] protected int actionPoints;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;

    [SerializeField] private Transform confirmPage;
    [SerializeField] private GameObject confirmBtn;

    [SerializeField] public GameObject buttonPrefab;


    protected bool isAOE;

    public virtual void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, confirmPage, confirmBtn, cancelBtn);
    }

    protected virtual void ConfigureButton(Button button, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        button.onClick.RemoveAllListeners();
        confirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            confirmPage.gameObject.SetActive(true);
        });

        cancelBtn.GetComponent<Button>().onClick.AddListener(() => {
            confirmPage.gameObject.SetActive(false);
        });
    }

    protected virtual void OnClicked(GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        Button confBtn = confirmBtn.gameObject.GetComponent<Button>();
        confBtn.onClick.AddListener(() => {
            confirmPage.SetActive(false);
            confirmBtn.SetActive(false);
            PlayerTurn.Instance.BasicMove();
        });
    }

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
