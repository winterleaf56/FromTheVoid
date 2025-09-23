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
            SerializedProperty skills = group.FindPropertyRelative("skills");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(unitName, new GUIContent("Unit Name"));

            // Moves Section
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

            // Actions Section
            EditorGUILayout.PropertyField(actions, new GUIContent("Actions"), true);

            // Skills Section
            EditorGUILayout.LabelField("Skills", EditorStyles.boldLabel);
            for (int k = 0; k < skills.arraySize; k++) {
                SerializedProperty skillEntry = skills.GetArrayElementAtIndex(k);
                SerializedProperty skillType = skillEntry.FindPropertyRelative("skillType");
                SerializedProperty skill = skillEntry.FindPropertyRelative("skill");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(skillType, GUIContent.none, GUILayout.MaxWidth(80));
                EditorGUILayout.PropertyField(skill, GUIContent.none);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    skills.DeleteArrayElementAtIndex(k);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ Add Skill")) {
                skills.InsertArrayElementAtIndex(skills.arraySize);
            }

            // Remove Group Button (Refers to all the above)
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