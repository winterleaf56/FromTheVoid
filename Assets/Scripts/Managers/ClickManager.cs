using UnityEngine;

public class ClickManager : MonoBehaviour {

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    // Sent to PlayerTurn to determine which unit the moves will be performed by
    private GameObject unitToSend = null;

    private GameObject lastEnemyClicked = null;
    private GameObject lastFriendlyClicked = null;

    public static ClickManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= doubleClickThreshold) {
                DetectDoubleClick();
            } else {
                //DetectClick();
            }
            lastClickTime = Time.time;
        }
    }

    // Click on a unit to select it
    // Currently configured to work exclusively with PlayerTurn
    // Can add a bool parameter doubleClicked with if statements in each block so the other method is not needed.
    public GameObject DetectClick() {
        if (!BattleManager.Instance.currentTurn.Equals(BattleManager.GameState.PlayerTurn)) {
            return null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            GameObject clickedObject = hit.collider.gameObject;

            /*if (BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack)) {
                if (clickedObject.CompareTag("Enemy")) {
                    print("Enemy Unit Clicked. Sent through PlayerAttack BattleState");
                    return unitToSend;
                }
            }*/

            if (lastFriendlyClicked != null) {
                lastFriendlyClicked.GetComponent<Light>().enabled = false;
            }

            if (!BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack)) {
                if (clickedObject.CompareTag("Enemy")) {
                    Debug.Log("Enemy Unit clicked");
                    //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Enemy>().health.GetHealth(), clickedObject.GetComponent<Enemy>().damage);
                    //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Enemy>().damage, clickedObject.GetComponent<MeleeEnemy>().actionPoints);
                    BattleManager.Instance.UnitClicked(false);
                    return unitToSend;
                } else if (clickedObject.CompareTag("Friendly")) {
                    Debug.Log("Friendly Unit clicked");
                    unitToSend = clickedObject;

                    if (clickedObject.GetComponent<Light>() != null) {
                        clickedObject.GetComponent<Light>().enabled = true;
                        lastFriendlyClicked = clickedObject;
                    }
                    //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Friendly>().health.GetHealth(), clickedObject.GetComponent<Friendly>().damage);
                    //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Friendly>().damage, clickedObject.GetComponent<Friendly>().actionPoints);
                    BattleManager.Instance.UnitClicked(true);
                    return unitToSend;
                } else if (clickedObject.CompareTag("UI")) {
                    Debug.Log("UI Clicked");
                    return unitToSend;
                } else {
                    Debug.Log("Non-unit object clicked");
                    //BattleManager.Instance.HideUnitStats();
                    BattleManager.Instance.UnitClicked(false);
                    return unitToSend;
                }
            } else {
                if (lastEnemyClicked != null) {
                    print("Last Clicked: " + lastEnemyClicked.name);
                    lastEnemyClicked.GetComponent<Light>().color = Color.green;
                }

                if (clickedObject.CompareTag("Enemy")) {
                    print("Enemy Unit Clicked. Sent through PlayerAttack BattleState");
                    clickedObject.GetComponent<Light>().color = Color.red;
                    lastEnemyClicked = clickedObject;
                    return unitToSend;
                }
            }


            /*if (clickedObject.GetComponent<Enemy>() != null) {
                Debug.Log("Enemy Unit clicked");
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Enemy>().health.GetHealth(), clickedObject.GetComponent<Enemy>().damage);
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Enemy>().damage, clickedObject.GetComponent<MeleeEnemy>().actionPoints);
                BattleManager.Instance.UnitClicked(false);
            } else if (clickedObject.GetComponent<Friendly>() != null) {
                Debug.Log("Friendly Unit clicked");
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Friendly>().health.GetHealth(), clickedObject.GetComponent<Friendly>().damage);
                //BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Friendly>().damage, clickedObject.GetComponent<Friendly>().actionPoints);
                BattleManager.Instance.UnitClicked(true);
            } else {
                Debug.Log("Non-unit object clicked");
                //BattleManager.Instance.HideUnitStats();
                BattleManager.Instance.UnitClicked(false);
            }*/
        }

        return unitToSend;
    }

    // Double click on any unit to show stats
    // Works anywhere as long as it is the Player's Turn
    void DetectDoubleClick() {
        if (!BattleManager.Instance.currentTurn.Equals(BattleManager.GameState.PlayerTurn)) {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.GetComponent<Enemy>() != null) {
                Debug.Log("Enemy Unit double-clicked");
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Enemy>().stats.MaxHealth, clickedObject.GetComponent<MeleeEnemy>().actionPoints);
            } else if (clickedObject.GetComponent<Friendly>() != null) {
                Debug.Log("Friendly Unit double-clicked");
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Friendly>().stats.MaxHealth, clickedObject.GetComponent<Friendly>().actionPoints);
            } else {
                Debug.Log("Non-unit object double-clicked");
                BattleManager.Instance.HideUnitStats();
            }
        }
    }
}
