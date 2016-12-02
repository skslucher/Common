using UnityEngine;
using UnityEditor;

//http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: @skslucher

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public virtual bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = true;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
        if (sourcePropertyValue != null)
        {
            enabled = CheckPropertyType(sourcePropertyValue);
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        SerializedProperty sourcePropertyValue2 = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField2);
        if (sourcePropertyValue2 != null)
        {
            enabled = enabled && CheckPropertyType(sourcePropertyValue2);
        }
        else
        {
            //Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        if (condHAtt.Inverse) enabled = !enabled;

        return enabled;
    }

    public virtual bool CheckPropertyType(SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue;
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
}



[CustomPropertyDrawer(typeof(ConditionalHideInterfaceAttribute), true)]
public class ConditionalHideInterfacePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideInterfaceAttribute condHAtt = (ConditionalHideInterfaceAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        //if (!condHAtt.HideInInspector || enabled)
        if(condHAtt.Behavior != ConditionalHideBehavior.Hide || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideInterfaceAttribute condHAtt = (ConditionalHideInterfaceAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        //if (!condHAtt.HideInInspector || enabled)
        if (condHAtt.Behavior != ConditionalHideBehavior.Hide || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public virtual bool GetConditionalHideAttributeResult(ConditionalHideInterfaceAttribute condHAtt, SerializedProperty property)
    {
        return condHAtt.GetConditionalHideAttributeResult(property);
    }
}



[CustomPropertyDrawer(typeof(ConditionalHideIntCustomDisplayAttribute), true)]
public class ConditionalHideIntCustomDisplayPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideIntCustomDisplayAttribute condHAtt = (ConditionalHideIntCustomDisplayAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        //if (!condHAtt.HideInInspector || enabled)
        if (condHAtt.Behavior != ConditionalHideBehavior.Hide || enabled)
        {
            property.isExpanded = true;

            SerializedProperty endProperty = property.GetEndProperty();

            switch (condHAtt.DisplayMode)
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

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideIntCustomDisplayAttribute condHAtt = (ConditionalHideIntCustomDisplayAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        //if (!condHAtt.HideInInspector || enabled)
        if (condHAtt.Behavior != ConditionalHideBehavior.Hide || enabled)
        {
            //return EditorGUI.GetPropertyHeight(property, label);
            property.isExpanded = true;

            float bodyHeight = (EditorGUI.GetPropertyHeight(property, label, true) - EditorGUI.GetPropertyHeight(property, label, false) - EditorGUIUtility.standardVerticalSpacing);
            
            switch (condHAtt.DisplayMode)
            {
                case CustomDisplayMode.NoLabel:
                    return bodyHeight;
                case CustomDisplayMode.LabelAsHeader:
                    return bodyHeight + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                case CustomDisplayMode.NoDropdown:
                    return EditorGUI.GetPropertyHeight(property, label, true);
                default:
                    return 0f;
            }
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    public virtual bool GetConditionalHideAttributeResult(ConditionalHideInterfaceAttribute condHAtt, SerializedProperty property)
    {
        return condHAtt.GetConditionalHideAttributeResult(property);
    }
}
/*
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
*/