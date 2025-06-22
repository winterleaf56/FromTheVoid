using UnityEngine;
using UnityEditor;

// Attribute class to add descriptions to fields
public class InspectorDescriptionAttribute : PropertyAttribute {
    public string Description;

    public InspectorDescriptionAttribute(string description) {
        Description = description;
    }
}


[CustomPropertyDrawer(typeof(InspectorDescriptionAttribute))]
public class InspectorDescriptionDrawer : PropertyDrawer {
    private const float indentWidth = 15f; // Adjust indentation as needed
    private const float verticalSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        InspectorDescriptionAttribute descriptionAttribute = (InspectorDescriptionAttribute)attribute;

        // First draw the property normally (variable name + field)
        float propertyHeight = EditorGUI.GetPropertyHeight(property, label);
        Rect propertyRect = new Rect(
            position.x,
            position.y,
            position.width,
            propertyHeight
        );
        EditorGUI.PropertyField(propertyRect, property, label, true);

        // Then draw the indented description below it
        Rect descriptionRect = new Rect(
            position.x + indentWidth, // Indent from left
            position.y + propertyHeight + verticalSpacing,
            position.width - indentWidth, // Reduce width to account for indent
            EditorGUIUtility.singleLineHeight
        );

        // Style the help box with padding and word wrap
        var style = new GUIStyle(EditorStyles.helpBox) {
            wordWrap = true,
            padding = new RectOffset(6, 6, 4, 4)
        };

        EditorGUI.LabelField(descriptionRect, descriptionAttribute.Description, style);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Calculate total height: Property + Spacing + Description
        float propertyHeight = base.GetPropertyHeight(property, label);
        float descriptionHeight = EditorStyles.helpBox.CalcHeight(
            new GUIContent(((InspectorDescriptionAttribute)attribute).Description),
            EditorGUIUtility.currentViewWidth - indentWidth // Account for indent
        );

        return propertyHeight + descriptionHeight + verticalSpacing;
    }
}

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