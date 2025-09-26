using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    //[SerializeField] private GameObject unit;
    [SerializeField] private Friendly unit;
    //private int unitID;

    private bool clicked = false;

    private void OnEnable() {
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void SetUnit(GameObject unit) {
        this.unit = unit;
    }*/

    public void SetUnit(Friendly unit) {
        this.unit = unit;
    }

    // V1.4.2 - Changed to use unit ID instead of the whole unit object
    // Using the FriendlyUnitID is easier than changing the whole system I've already made.
    public void OnClick() {
        if (!clicked) {
            //GameManager.Instance.SelectedLevel.AddPlayerUnit(unit);
            GameManager.Instance.SelectedLevel.AddPlayerUnitByID(unit.FriendlyUnitID);
            transform.Find("Selected").gameObject.SetActive(true);
            Debug.Log("Added unit");
            GameManager.unitSelected(1);
            clicked = true;
        } else {
            //GameManager.Instance.SelectedLevel.RemovePlayerUnit(unit);
            GameManager.Instance.SelectedLevel.RemovePlayerUnitByID(unit.FriendlyUnitID);
            transform.Find("Selected").gameObject.SetActive(false);
            Debug.Log("Removed unit");
            GameManager.unitSelected(-1);
            clicked = false;
        }
    }
}
