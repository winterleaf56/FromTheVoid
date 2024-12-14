using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour {

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    [SerializeField] private GameObject markerPrefab;
    private GameObject markerInstance;
    private Coroutine followMouseCoroutine;

    // Sent to PlayerTurn to determine which unit the moves will be performed by
    private GameObject unitToSend = null;

    // Used to keep track of the last unit clicked for activating the lights on each unit
    private GameObject lastEnemyClicked = null;
    private GameObject lastFriendlyClicked = null;

    public bool allowClicks = true;

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
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            if (allowClicks == false) return;

            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= doubleClickThreshold) {
                DetectDoubleClick();
            } else {
                //DetectClick();
            }
            lastClickTime = Time.time;
        }
    }

    // Add a way to turn off the unit lights by clicking anywhere other than another unit

    // Click on a unit to select it
    // Currently configured to work exclusively with PlayerTurn
    // Can add a bool parameter doubleClicked with if statements in each block so the other method is not needed.
    public GameObject DetectClick() {
        if (!BattleManager.Instance.currentTurn.Equals(BattleManager.GameState.PlayerTurn)) {
            return null;
        }

        if (!allowClicks) return null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            GameObject clickedObject = hit.collider.gameObject;

            if (!BattleManager.Instance.currentBattleState.Equals(BattleManager.BattleState.PlayerAttack)) {
                if (clickedObject.CompareTag("Enemy")) {
                    Debug.Log("Enemy Unit clicked");
                    BattleManager.Instance.UnitClicked(false);
                    unitToSend = clickedObject;
                    return unitToSend;
                } else if (clickedObject.CompareTag("Friendly")) {
                    Debug.Log("Friendly Unit clicked");
                    unitToSend = clickedObject;

                    if (clickedObject.GetComponent<Light>() != null) {

                        if (lastFriendlyClicked != null) lastFriendlyClicked.GetComponent<Light>().enabled = false;

                        clickedObject.GetComponent<Light>().enabled = true;
                        lastFriendlyClicked = clickedObject;
                    }
                    BattleManager.Instance.UnitClicked(true);
                    return unitToSend;
                } else if (clickedObject.CompareTag("UI")) {
                    Debug.Log("UI Clicked");
                    
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
                    return lastEnemyClicked;
                } else {
                    print("Non-Enemy Unit Clicked. Sent through PlayerAttack BattleState");
                    return lastEnemyClicked;
                }
            }
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
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Enemy>().stats.MaxHealth, clickedObject.GetComponent<MeleeEnemy>().stats.ActionPoints);
            } else if (clickedObject.GetComponent<Friendly>() != null) {
                Debug.Log("Friendly Unit double-clicked");
                BattleManager.Instance.ShowUnitStats(clickedObject.GetComponent<Health>().GetHealth(), clickedObject.GetComponent<Friendly>().stats.MaxHealth, clickedObject.GetComponent<Friendly>().stats.ActionPoints);
            } else {
                Debug.Log("Non-unit object double-clicked");
                BattleManager.Instance.HideUnitStats();
            }
        }
    }

    public void FindMovePosition(System.Action<Vector3> callback) {
        if (markerInstance == null) {
            markerInstance = Instantiate(markerPrefab);
        }
        

        followMouseCoroutine = StartCoroutine(FollowMouse(callback));
    }

    private IEnumerator FollowMouse(System.Action<Vector3> callback) {
        Vector3 lastMousePos = Vector3.zero;
        int layerMask = ~LayerMask.GetMask("IgnoreRaycast");

        while (true) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                if (Vector3.Distance(hit.point, lastMousePos) > 0.1f) {
                    markerInstance.transform.position = hit.point;
                    lastMousePos = hit.point;
                }

                if (Input.GetMouseButtonDown(0)) {
                    Vector3 placedPosition = markerInstance.transform.position;
                    Instantiate(markerPrefab, placedPosition, Quaternion.identity);
                    Destroy(markerInstance.gameObject);
                    markerInstance = null;
                    followMouseCoroutine = null;
                    callback(placedPosition);
                    yield break;
                }
            }
            yield return null;
        }
    }
}
