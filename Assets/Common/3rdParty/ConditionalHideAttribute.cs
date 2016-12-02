using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


//http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: @skslucher

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public string ConditionalSourceField2 = "";
    public bool HideInInspector = false;
    public bool Inverse = false;

    // Use this for initialization
    public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.Inverse = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = inverse;
    }
}



//My modification


public enum ConditionalHideBehavior { Disable, Hide }


public class ConditionalHideInterfaceAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    
    public ConditionalHideBehavior Behavior = ConditionalHideBehavior.Disable;


#if UNITY_EDITOR
    public virtual bool GetConditionalHideAttributeResult(SerializedProperty property)
    {
        bool enabled = true;
        
        //SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.FindSibling(ConditionalSourceField);
        if (sourcePropertyValue != null)
        {
            enabled = IsEnabledValue(sourcePropertyValue);
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + ConditionalSourceField);
        }

        return enabled;
    }
    
    public virtual bool IsEnabledValue(SerializedProperty sourcePropertyValue)
    {
        return false;
    }
#endif
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideBoolAttribute : ConditionalHideInterfaceAttribute
{
    public bool EnabledValue;

    public ConditionalHideBoolAttribute(string conditionalSourceField, bool enabledValue, ConditionalHideBehavior behavior = ConditionalHideBehavior.Disable)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnabledValue = enabledValue;
        this.Behavior = behavior;
    }
    

#if UNITY_EDITOR
    public override bool IsEnabledValue(SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue == EnabledValue;
            case SerializedPropertyType.ObjectReference:
                return (sourcePropertyValue.objectReferenceValue != null) == EnabledValue;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
#endif
}


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideIntAttribute : ConditionalHideInterfaceAttribute
{
    public int EnabledValue;

    public ConditionalHideIntAttribute(string conditionalSourceField, int enabledValue, ConditionalHideBehavior behavior = ConditionalHideBehavior.Disable)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnabledValue = enabledValue;
        this.Behavior = behavior;
    }

#if UNITY_EDITOR
    public override bool IsEnabledValue(SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Integer:
                return sourcePropertyValue.intValue == EnabledValue;
            case SerializedPropertyType.Enum:
                return sourcePropertyValue.enumValueIndex == EnabledValue;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
#endif
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideIntCustomDisplayAttribute : ConditionalHideIntAttribute
{
    public CustomDisplayMode DisplayMode;
    
    public ConditionalHideIntCustomDisplayAttribute(string conditionalSourceField, int enabledValue, ConditionalHideBehavior behavior = ConditionalHideBehavior.Disable, CustomDisplayMode displayMode = CustomDisplayMode.NoLabel) : base(conditionalSourceField, enabledValue, behavior)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnabledValue = enabledValue;
        this.Behavior = behavior;
        this.DisplayMode = displayMode;
    }
}



#if UNITY_EDITOR

public static class SerializedPropertyExtension
{

    public static SerializedProperty FindSibling(this SerializedProperty property, string siblingPropertyName)
    {
        string[] pathSplit = property.propertyPath.Split('.');
        
        pathSplit[pathSplit.Length - 1] = siblingPropertyName;
        
        return property.serializedObject.FindProperty(string.Join(".", pathSplit));
    }



}


#endif