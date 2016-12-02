using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomPropertyDrawer(typeof(ListAdvancedAttribute))]
public class ListAdvancedDrawer : ListWrapperDrawer
{
    public virtual bool isFixedLength
    {
        get
        {
            ListAdvancedAttribute listAdvanced = attribute as ListAdvancedAttribute;
            return listAdvanced.isFixedLength;
        }
    }
    public virtual int listLength
    {
        get
        {
            ListAdvancedAttribute listAdvanced = attribute as ListAdvancedAttribute;
            return listAdvanced.length;
        }
    }
    public virtual string[] listLabels
    {
        get
        {
            ListAdvancedAttribute listAdvanced = attribute as ListAdvancedAttribute;
            return listAdvanced.labels;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property = GetDisplayProperty(property);

        position.height = EditorGUI.GetPropertyHeight(property, label, false);

        EditorGUI.PropertyField(position, property, label, false);

        position.y += EditorGUI.GetPropertyHeight(property, label, false) + EditorGUIUtility.standardVerticalSpacing;

        if (property.isArray && property.isExpanded)
        {
            EditorGUI.indentLevel++;

            if (isFixedLength)
            {
                property.arraySize = listLength;
            }
            else
            {
                EditorGUI.BeginProperty(position, label, property);
                property.arraySize = EditorGUI.DelayedIntField(position, new GUIContent("Size"), property.arraySize);
                EditorGUI.EndProperty();
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty child = property.GetArrayElementAtIndex(i);

                GUIContent childLabel;
                if (listLabels != null && i < listLabels.Length)
                {
                    childLabel = new GUIContent(listLabels[i]);
                }
                else
                {
                    childLabel = new GUIContent("Element " + i);
                }


                EditorGUI.PropertyField(position, child, childLabel, true);
                position.y += EditorGUI.GetPropertyHeight(child, childLabel, true) + EditorGUIUtility.standardVerticalSpacing;

            }

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        property = GetDisplayProperty(property);

        float height = 0f;

        height += EditorGUI.GetPropertyHeight(property, label, false);

        if (property.isExpanded)
        {
            if (isFixedLength)
            {
                property.arraySize = listLength;
            }
            else
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty child = property.GetArrayElementAtIndex(i);

                GUIContent childLabel;// = new GUIContent(child.name);
                if (listLabels != null && i < listLabels.Length)
                {
                    childLabel = new GUIContent(listLabels[i]);
                }
                else
                {
                    childLabel = new GUIContent("Element " + i);
                }

                height += EditorGUI.GetPropertyHeight(child, childLabel, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }
}

[CustomPropertyDrawer(typeof(GenericListWrapper), true)]
public class ListWrapperDrawer : ReplaceDisplayPropertyDrawer
{
    public override string relativePropertyPath { get { return "list"; } }
}