using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartTesting))]
public class StartTestingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get reference to the target script
        StartTesting startTesting = (StartTesting)target;

        // Only show the button if the game is playing
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Set GameManager.SelectedLevel to testingLevel"))
            {
                // Set the selected level in GameManager
                GameManager.Instance.SetSelectedLevel(startTesting.TestingLevel);
                Debug.Log("GameManager.SelectedLevel set to testingLevel.");

                StartTesting.Instance.LoadTestUnits();
                //GameManager.Instance.StartTestingLevel();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Button only available in Play mode.", MessageType.Info);
        }
    }
}