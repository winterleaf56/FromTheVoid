using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "RewardIconDatabase", menuName = "Rewards/Reward Icon Database")]
public class RewardIconDatabase : ScriptableObject {
    private static RewardIconDatabase _instance;
    public static RewardIconDatabase Instance {
        get {
            if (_instance == null) {
                // Try to load the database from Resources
                _instance = Resources.Load<RewardIconDatabase>("Rewards/RewardIconDatabase");

#if UNITY_EDITOR
                // Auto-create in Editor if it doesn't exist
                if (_instance == null) {
                    Debug.Log("Creating new RewardIconDatabase...");
                    _instance = CreateInstance<RewardIconDatabase>();

                    // Ensure folders exist
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    if (!AssetDatabase.IsValidFolder("Assets/Resources/Rewards"))
                        AssetDatabase.CreateFolder("Assets/Resources", "Rewards");

                    // Save the asset
                    string path = "Assets/Resources/Rewards/RewardIconDatabase.asset";
                    AssetDatabase.CreateAsset(_instance, path);
                    AssetDatabase.SaveAssets();
                    Debug.Log($"Saved new RewardIconDatabase at: {path}");
                }
#endif
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class RewardIconPair {
        public RewardType rewardType;
        public Sprite icon;
    }

    [SerializeField] private List<RewardIconPair> iconMappings = new List<RewardIconPair>();
    private Dictionary<RewardType, Sprite> _iconLookup;

    private void BuildLookup() {
        _iconLookup = new Dictionary<RewardType, Sprite>();
        foreach (var pair in iconMappings) {
            _iconLookup[pair.rewardType] = pair.icon;
        }
    }

    public Sprite GetIcon(RewardType type) {
        if (_iconLookup == null || _iconLookup.Count == 0)
            BuildLookup();

        return _iconLookup.TryGetValue(type, out Sprite icon) ? icon : null;
    }
}