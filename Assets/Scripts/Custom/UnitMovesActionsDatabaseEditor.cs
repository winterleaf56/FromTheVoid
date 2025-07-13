using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitMovesActionsDatabase))]
public class UnitMovesActionsDatabaseEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty unitGroups = serializedObject.FindProperty("unitGroups");

        if (GUILayout.Button("+ Add Unit Group")) {
            unitGroups.InsertArrayElementAtIndex(unitGroups.arraySize);
        }

        for (int i = 0; i < unitGroups.arraySize; i++) {
            SerializedProperty group = unitGroups.GetArrayElementAtIndex(i);
            SerializedProperty unitName = group.FindPropertyRelative("unitName");
            SerializedProperty moves = group.FindPropertyRelative("moves");
            SerializedProperty actions = group.FindPropertyRelative("actions");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(unitName, new GUIContent("Unit Name"));

            EditorGUILayout.LabelField("Moves", EditorStyles.boldLabel);
            for (int j = 0; j < moves.arraySize; j++) {
                SerializedProperty moveEntry = moves.GetArrayElementAtIndex(j);
                SerializedProperty moveType = moveEntry.FindPropertyRelative("moveType");
                SerializedProperty move = moveEntry.FindPropertyRelative("move");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(moveType, GUIContent.none, GUILayout.MaxWidth(80));
                EditorGUILayout.PropertyField(move, GUIContent.none);
                if (GUILayout.Button("-", GUILayout.Width(20))) {
                    moves.DeleteArrayElementAtIndex(j);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("+ Add Move")) {
                moves.InsertArrayElementAtIndex(moves.arraySize);
            }

            EditorGUILayout.PropertyField(actions, new GUIContent("Actions"), true);

            if (GUILayout.Button("Remove Group")) {
                unitGroups.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("", MessageType.None);
            EditorGUILayout.Space(10);
        }

        serializedObject.ApplyModifiedProperties();
    }
}