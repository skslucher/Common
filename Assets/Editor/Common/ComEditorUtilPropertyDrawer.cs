using UnityEngine;
using UnityEditor;
using System.Collections;


public static class PropertyDrawerUtil
{
    public static Rect DrawProperty(SerializedProperty property, Rect position, bool includeChildren = true)
    {
        float height = EditorGUI.GetPropertyHeight(property, GUIContent.none, includeChildren);
        position.height = height;
        EditorGUI.PropertyField(position, property, includeChildren);
        position.y += height + EditorGUIUtility.standardVerticalSpacing;
        return position;
    }

    public static Rect DrawLabel(GUIContent label, Rect position)
    {
        return DrawLabel(label, position, GUIStyle.none);
    }

    public static Rect DrawLabel(GUIContent label, Rect position, GUIStyle style)
    {
        float height = EditorGUIUtility.singleLineHeight;
        position.height = height;
        EditorGUI.LabelField(position, label, style);
        position.y += height + EditorGUIUtility.standardVerticalSpacing;
        return position;
    }
}



[CustomPropertyDrawer(typeof(ReplaceDisplayPropertyAttribute))]
public class ReplaceDisplayPropertyDrawer : PropertyDrawer
{
    public virtual string relativePropertyPath
    {
        get
        {
            ReplaceDisplayPropertyAttribute displayProperty = attribute as ReplaceDisplayPropertyAttribute;
            return displayProperty.relativePropertyPath;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, GetDisplayProperty(property), label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(GetDisplayProperty(property), label, true);
    }

    public SerializedProperty GetDisplayProperty(SerializedProperty property)
    {
        SerializedProperty subObject = property.FindPropertyRelative(relativePropertyPath);
        if (subObject == null)
        {
            Debug.Log("Error! Property not found: " + relativePropertyPath);
            subObject = property;
        }

        return subObject;
    }
}



[CustomPropertyDrawer(typeof(CustomDisplayAttribute))]
public class CustomDisplayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomDisplayAttribute customDisplay = attribute as CustomDisplayAttribute;
        property.isExpanded = true;

        SerializedProperty endProperty = property.GetEndProperty();

        switch (customDisplay.displayMode)
        {
            case CustomDisplayMode.NoLabel:
                break;
            case CustomDisplayMode.LabelAsHeader:
                position = PropertyDrawerUtil.DrawLabel(label, position, EditorStyles.boldLabel);
                break;
            case CustomDisplayMode.NoDropdown:
                position = PropertyDrawerUtil.DrawLabel(label, position);
                break;

        }

        property.NextVisible(true);

        do
        {
            position = PropertyDrawerUtil.DrawProperty(property, position, true);

            property.NextVisible(false);

        } while (!SerializedProperty.EqualContents(property, endProperty));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        property.isExpanded = true;

        float bodyHeight = (EditorGUI.GetPropertyHeight(property, label, true) - EditorGUI.GetPropertyHeight(property, label, false) - EditorGUIUtility.standardVerticalSpacing);

        CustomDisplayAttribute customDisplay = attribute as CustomDisplayAttribute;
        switch (customDisplay.displayMode)
        {
            case CustomDisplayMode.NoLabel:
                return bodyHeight;
            case CustomDisplayMode.LabelAsHeader:
                return bodyHeight + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            case CustomDisplayMode.NoDropdown:
                return EditorGUI.GetPropertyHeight(property, label, true);
        }

        return 0f;
    }
}


[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}