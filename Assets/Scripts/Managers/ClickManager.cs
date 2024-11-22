using UnityEngine;

public class ClickManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            DetectClick();
        }
    }

    void DetectClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.GetComponent<Enemy>() != null) {
                Debug.Log("Enemy Unit clicked");
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Enemy>().health.GetHealth(), clickedObject.GetComponent<Enemy>().damage);
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Enemy>().damage, clickedObject.GetComponent<Enemy>().actionPoints);
            } else if (clickedObject.GetComponent<Friendly>() != null) {
                Debug.Log("Friendly Unit clicked");
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Friendly>().health.GetHealth(), clickedObject.GetComponent<Friendly>().damage);
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Friendly>().damage, clickedObject.GetComponent<Friendly>().actionPoints);
            } else {
                Debug.Log("Non-unit object clicked");
                BattleManager.Instance.HideUnitStats();
            }
        }
    }
}
