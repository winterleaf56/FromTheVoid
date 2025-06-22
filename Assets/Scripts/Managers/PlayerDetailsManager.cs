using UnityEngine;

public class PlayerDetailsManager : MonoBehaviour {

    [SerializeField] private string playerName;
    [SerializeField] private int currentVoidShards;
    public string PlayerName => playerName;
    //public int CurrentVoidShards => currentVoidShards;

    /*public int VoidShards {
        get => currentVoidShards;
        private set {
            currentVoidShards = value;
            // Here you can add logic to update UI or notify other systems
        }
    }*/

    public static PlayerDetailsManager Instance;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddVoidShards(int value) {
        currentVoidShards += value;
    }

    public void SpendVoidShards(int value) {
        currentVoidShards -= value;
    }

    public int GetCurrentVoidShards() {
        return currentVoidShards;
    }

    public void SetPlayerName(string name) {
        playerName = name;

        Debug.LogAssertion($"Player name set to: {playerName}. Saving...");
        SaveManager.Instance.SaveGame?.Invoke();
    }
}
