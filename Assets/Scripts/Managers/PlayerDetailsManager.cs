using UnityEngine;
using static BattleManager;

public class PlayerDetailsManager : MonoBehaviour {

    [SerializeField] private string playerName;
    [SerializeField] private int playerId; // Will be assigned by the server in ascending order.
    [SerializeField] private int playerRank;
    [SerializeField] private int currentVoidShards;
    [SerializeField] private int currentCoins;

    public string PlayerName => playerName;
    public int PlayerId => playerId;
    public int PlayerRank => playerRank;
    //public int CurrentVoidShards => currentVoidShards;

    private int CurrentVoidShards {
        get => currentVoidShards;
        set {
            currentVoidShards = value;
            // Here you can add logic to update UI or notify other systems
            Debug.Log($"Current Void Shards updated to: {currentVoidShards}");
            MenuUIManager.updateUserDetails?.Invoke();
            SaveManager.Instance.SavePlayerData();
        }
    }

    private int CurrentCoins {
        get => currentCoins;
        set {
            currentCoins = value;
            // Here you can add logic to update UI or notify other systems
            Debug.Log($"Current Coins updated to: {currentCoins}");
            MenuUIManager.updateUserDetails?.Invoke();
            SaveManager.Instance.SavePlayerData();
        }
    }

    /*public int VoidShards {
        get => currentVoidShards;
        private set {
            currentVoidShards = value;
            // Here you can add logic to update UI or notify other systems
        }
    }*/

    public static PlayerDetailsManager Instance;

    void Awake() {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        //SaveManager.Instance.LoadGame?.Invoke();
    }

    // -- Void Shards -- //
    public void AddVoidShards(int value) {
        Debug.Log($"Adding {value} Void Shards to current Void Shards: {currentVoidShards}");
        CurrentVoidShards += value;
    }

    public void SpendVoidShards(int value) {
        CurrentVoidShards -= value;
    }

    public void SetVoidShards(int value) {
        CurrentVoidShards = value;
        Debug.Log($"Setting Void Shards to: {CurrentVoidShards}");
    }

    public int GetCurrentVoidShards() {
        return currentVoidShards;
    }
    // -- Void Shards -- //

    // -- Coins -- //
    public void AddCoins(int value) {
        CurrentCoins += value;
    }

    public void SpendCoins(int value) {
        CurrentCoins -= value;
    }

    public void SetCoins(int value) {
        CurrentCoins = value;
    }

    public int GetCurrentCoins() {
        return currentCoins;
    }
    // -- Coins -- //

    public void SetPlayerName(string name) {
        playerName = name;

        Debug.LogAssertion($"Player name set to: {playerName}. Saving...");
        SaveManager.Instance.SaveGame?.Invoke();
    }
}
