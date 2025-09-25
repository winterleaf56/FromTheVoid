#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CustomReward))]
public class CustomRewardEditor : Editor {
    private SerializedProperty rewardsProperty;
    private ReorderableList reorderableList;

    private void OnEnable() {
        rewardsProperty = serializedObject.FindProperty("rewards");
        reorderableList = new ReorderableList(serializedObject, rewardsProperty, true, true, true, true) {
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Rewards"),
            drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = rewardsProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;

                // RewardType dropdown (fixed width)
                float typeWidth = 100f;
                var rewardTypeRect = new Rect(rect.x, rect.y, typeWidth, rect.height);
                EditorGUI.PropertyField(rewardTypeRect, element.FindPropertyRelative("rewardType"), GUIContent.none);

                // Space between fields
                float spacing = 10f;

                // Field for amount/itemReference (use remaining width minus some padding)
                float fieldWidth = rect.width - typeWidth - spacing;
                var fieldRect = new Rect(rect.x + typeWidth + spacing, rect.y, fieldWidth, rect.height);

                var rewardType = (RewardType)element.FindPropertyRelative("rewardType").enumValueIndex;

                switch (rewardType)
                {
                    case RewardType.VoidShards:
                    case RewardType.Coins:
                        EditorGUI.PropertyField(fieldRect, element.FindPropertyRelative("amount"), new GUIContent("Amount"));
                        break;
                    case RewardType.Item:
                        EditorGUI.PropertyField(fieldRect, element.FindPropertyRelative("itemReference"), new GUIContent("Item"));
                        break;
                    case RewardType.Unit:
                        EditorGUI.PropertyField(fieldRect, element.FindPropertyRelative("unitReference"), new GUIContent("Unit"));
                        break;
                }
            }
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif

// This is a seemingly complex yet working way to make the images appear in the editor next to the reward types.

/*[CustomEditor(typeof(CustomReward))]
public class CustomRewardEditor : Editor {
    private SerializedProperty rewardsProperty;
    private ReorderableList reorderableList;
    private RewardIconDatabase iconDatabase; // Cache the icon database

    private void OnEnable() {
        rewardsProperty = serializedObject.FindProperty("rewards");
        iconDatabase = RewardIconDatabase.Instance; // Load the database once

        reorderableList = new ReorderableList(serializedObject, rewardsProperty, true, true, true, true) {
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Rewards"),
            drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = rewardsProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight * 2; // Double height for icon + fields

                // Get reward type
                var rewardTypeProp = element.FindPropertyRelative("rewardType");
                RewardType rewardType = (RewardType)rewardTypeProp.enumValueIndex;

                // --- Draw Icon (New) ---
                if (iconDatabase != null) {
                    Sprite icon = iconDatabase.GetIcon(rewardType);
                    if (icon != null) {
                        Rect iconRect = new Rect(rect.x, rect.y, 20, 20);
                        GUI.DrawTexture(iconRect, icon.texture, ScaleMode.ScaleToFit);
                    }
                }

                // --- Draw Reward Type Dropdown ---
                Rect typeRect = new Rect(rect.x + 25, rect.y, rect.width * 0.35f, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(typeRect, rewardTypeProp, GUIContent.none);

                // --- Draw Fields (Amount/Item) ---
                Rect fieldRect = new Rect(rect.x + 25 + typeRect.width + 5, rect.y,
                                         rect.width * 0.6f - 30, EditorGUIUtility.singleLineHeight);

                switch (rewardType) {
                    case RewardType.VoidShards:
                    case RewardType.Coins:
                        EditorGUI.PropertyField(fieldRect, element.FindPropertyRelative("amount"), GUIContent.none);
                        break;
                    case RewardType.Item:
                        EditorGUI.PropertyField(fieldRect, element.FindPropertyRelative("itemReference"), GUIContent.none);
                        break;
                }

                // --- Draw Secondary Line (Optional: Icon Preview) ---
                if (Event.current.type == EventType.Repaint) {
                    Rect previewRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight,
                                              rect.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.DropShadowLabel(previewRect, $"{rewardType} (Icon Preview)");
                }
            },
            elementHeightCallback = index => EditorGUIUtility.singleLineHeight * 2 // Fixed 2-line height
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        // Optional: Button to force-refresh icons
        if (GUILayout.Button("Refresh Icons")) {
            iconDatabase = RewardIconDatabase.Instance;
        }
    }
}*/