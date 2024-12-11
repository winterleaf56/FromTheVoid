using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting.FullSerializer;

public abstract class ActionBase : ScriptableObject {
    public string moveName;
    [SerializeField] protected int actionPointCost;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;

    [SerializeField] private Transform confirmPage;
    [SerializeField] private GameObject confirmBtn;


    // Makes more sense to remove this replace it with an image maybe.
    [SerializeField] public GameObject buttonPrefab;


    protected bool isAOE;

    public virtual void SetupButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ConfigureButton(button, unit, confirmPage, confirmBtn, cancelBtn);
    }

    protected virtual void ConfigureButton(Button button, Lifeforms unit, GameObject confirmPage, GameObject confirmBtn, Button cancelBtn) {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;

        if (unit.stats.ActionPoints < GetAPRequirement()) {
            button.interactable = false;
            colors.normalColor = new Color(0.5f, 0.5f, 0.5f);
        }

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
        Debug.Log("INSIDE GET AP REQUIREMENT");
        return actionPointCost;
    }

    /*public virtual void Execute() {
        Debug.Log("Performing move: " + moveName);
    }*/
}
