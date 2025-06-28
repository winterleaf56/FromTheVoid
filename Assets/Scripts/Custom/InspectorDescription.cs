using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// This part can be used in builds (just the attribute definition)
public class InspectorDescriptionAttribute : PropertyAttribute {
    public string Description { get; private set; }

    public InspectorDescriptionAttribute(string description) {
        Description = description;
    }
}

#if UNITY_EDITOR

// The drawer implementation is editor-only
[CustomPropertyDrawer(typeof(InspectorDescriptionAttribute))]
public class InspectorDescriptionDrawer : PropertyDrawer {
    private const float INDENT_WIDTH = 15f;
    private const float VERTICAL_SPACING = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var descriptionAttribute = (InspectorDescriptionAttribute)attribute;

        // Draw property first
        Rect propertyRect = new Rect(
            position.x,
            position.y,
            position.width,
            EditorGUI.GetPropertyHeight(property, label)
        );
        EditorGUI.PropertyField(propertyRect, property, label, true);

        // Draw description below
        Rect descriptionRect = new Rect(
            position.x + INDENT_WIDTH,
            position.y + propertyRect.height + VERTICAL_SPACING,
            position.width - INDENT_WIDTH,
            EditorStyles.helpBox.CalcHeight(
                new GUIContent(descriptionAttribute.Description),
                position.width - INDENT_WIDTH
            )
        );

        EditorGUI.LabelField(descriptionRect, descriptionAttribute.Description, EditorStyles.helpBox);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var descriptionAttribute = (InspectorDescriptionAttribute)attribute;
        float propertyHeight = EditorGUI.GetPropertyHeight(property, label);
        float descriptionHeight = EditorStyles.helpBox.CalcHeight(
            new GUIContent(descriptionAttribute.Description),
            EditorGUIUtility.currentViewWidth - INDENT_WIDTH
        );

        return propertyHeight + descriptionHeight + VERTICAL_SPACING;
    }
}
#endif

// Property drawer to render the description in the Inspector
/*[CustomPropertyDrawer(typeof(InspectorDescriptionAttribute))]
public class InspectorDescriptionDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        InspectorDescriptionAttribute descriptionAttribute = (InspectorDescriptionAttribute)attribute;

        // Draw the description (using the original label as a header if you want)
        Rect descriptionRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(descriptionRect, new GUIContent(descriptionAttribute.Description), EditorStyles.helpBox);

        // Adjust position for property field - use original label
        Rect propertyRect = new Rect(
            position.x,
            position.y + EditorGUIUtility.singleLineHeight + 2,
            position.width,
            EditorGUI.GetPropertyHeight(property, label)
        );

        // Draw property with its original label
        EditorGUI.PropertyField(propertyRect, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight + 4;
    }
}*/