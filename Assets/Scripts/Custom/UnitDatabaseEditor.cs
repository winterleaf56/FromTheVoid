using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitDatabase))]
public class UnitDatabaseEditor : Editor {
    public override void OnInspectorGUI() {
        // Draw the default inspector for other fields
        DrawDefaultInspector();

        // Custom display for unitPrefabs
        UnitDatabase db = (UnitDatabase)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Unit Prefabs with IDs", EditorStyles.boldLabel);

        if (db.unitPrefabs != null) {
            for (int i = 0; i < db.unitPrefabs.Count; i++) {
                var unit = db.unitPrefabs[i];
                if (unit != null) {
                    EditorGUILayout.BeginHorizontal();
                    db.unitPrefabs[i] = (Friendly)EditorGUILayout.ObjectField(unit, typeof(Friendly), false);
                    EditorGUILayout.LabelField($"ID: {unit.FriendlyUnitID}", GUILayout.Width(80));
                    EditorGUILayout.EndHorizontal();
                } else {
                    db.unitPrefabs[i] = (Friendly)EditorGUILayout.ObjectField(null, typeof(Friendly), false);
                }
            }
        }
    }
}