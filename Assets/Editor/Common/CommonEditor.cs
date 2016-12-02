using UnityEngine;
using System.Collections;
using UnityEditor;





public class SerializedContent
{
    public enum CollapseBehavior
    {
        Hide,
        Show,
        AlwaysInHeader
    }

    public SerializedContent(SerializedProperty property, string name, CollapseBehavior behavior = CollapseBehavior.Hide, int sizeWeight = 1)
    {
        this.property = property;
        this.name = name;
        this.behavior = behavior;
        this.sizeWeight = sizeWeight;
    }

    public SerializedProperty property;
    public string name;
    public CollapseBehavior behavior;
    public int sizeWeight;
}

public class CollapsibleDrawer : PropertyDrawer
{
    const float buffer = 5f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position.height = EditorGUI.GetPropertyHeight(property, label, false);
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

        //property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, label);
        EditorGUI.PropertyField(labelRect, property, label, false);

        SerializedContent[] contents = GetContents(property);

        DrawHeader(position, contents, property);

        if (property.isExpanded)
        {
            DrawExpanded(position, contents);
        }

        EditorGUI.EndProperty();
    }    

    public void DrawHeader(Rect position, SerializedContent[] contents, SerializedProperty property)
    {
        position.xMin += EditorGUIUtility.labelWidth;

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int count = 0;
        int totalSizeWeight = 0;

        for(int i=0; i<contents.Length; i++)
        {
            if(contents[i].behavior == SerializedContent.CollapseBehavior.AlwaysInHeader || (!property.isExpanded && contents[i].behavior == SerializedContent.CollapseBehavior.Show))
            {
                count++;
                totalSizeWeight += contents[i].sizeWeight;
            }
        }

        /*count = CountContents(contents, SerializedContent.CollapseBehavior.AlwaysInHeader);
        if (!property.isExpanded)
        {
            count += CountContents(contents, SerializedContent.CollapseBehavior.Show);
        }*/

        //Rect[] rects = Com.SplitRect(position, count, buffer, true);
        Rect[] rects = Com.SplitRect(position, totalSizeWeight, buffer, true);

        int j = 0;
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].behavior == SerializedContent.CollapseBehavior.AlwaysInHeader || (!property.isExpanded && contents[i].behavior == SerializedContent.CollapseBehavior.Show))
            {
                if (contents[i].sizeWeight > 1)
                {
                    EditorGUI.PropertyField(Com.Combine(rects[j], rects[j + contents[i].sizeWeight - 1]), contents[i].property, GUIContent.none);
                    j += contents[i].sizeWeight;
                }
                else
                {
                    EditorGUI.PropertyField(rects[j], contents[i].property, GUIContent.none);
                    j++;
                }
            }
        }

        EditorGUI.indentLevel = indent;
    }

    public void DrawExpanded(Rect position, SerializedContent[] contents)
    {
        EditorGUI.indentLevel++;

        position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].behavior != SerializedContent.CollapseBehavior.AlwaysInHeader)
            {
                EditorGUI.PropertyField(position, contents[i].property, new GUIContent(contents[i].name));
                position.y += EditorGUI.GetPropertyHeight(contents[i].property, new GUIContent(contents[i].name), true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded)
        {
            SerializedContent[] contents = GetContents(property);
            foreach (SerializedContent content in contents)
            {
                if (content.behavior != SerializedContent.CollapseBehavior.AlwaysInHeader)
                {
                    height += EditorGUI.GetPropertyHeight(content.property, new GUIContent(content.name), true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        return height;
    }

    public int CountContents(SerializedContent[] contents, SerializedContent.CollapseBehavior behavior)
    {
        int count = 0;
        foreach(SerializedContent content in contents)
        {
            if(content.behavior == behavior)
            {
                count++;
            }
        }
        return count;
    }


    public virtual SerializedContent[] GetContents(SerializedProperty property)
    {
        return new SerializedContent[0];
    }
}


[CustomPropertyDrawer (typeof(RandomizedFloat))]
public class RandomizedFloatEditor : CollapsibleDrawer
{
    public override SerializedContent[] GetContents(SerializedProperty property)
    {
        SerializedContent[] contents = new SerializedContent[VisibleFields(property)];


        SerializedProperty randomType = property.FindPropertyRelative("variableType");

        int i = 0;
        switch (randomType.enumValueIndex)
        {
            case 0:
                contents[i] = new SerializedContent(property.FindPropertyRelative("baseValue"), "Constant", SerializedContent.CollapseBehavior.Show);
                i++;
                break;
            case 1:
                contents[i] = new SerializedContent(property.FindPropertyRelative("baseValue"), "Minimum", SerializedContent.CollapseBehavior.Show);
                i++;
                contents[i] = new SerializedContent(property.FindPropertyRelative("modifier"), "Maximum", SerializedContent.CollapseBehavior.Show);
                i++;
                break;
            case 2:
                contents[i] = new SerializedContent(property.FindPropertyRelative("baseValue"), "Base Value", SerializedContent.CollapseBehavior.Show);
                i++;
                contents[i] = new SerializedContent(property.FindPropertyRelative("modifier"), "Percent Variance", SerializedContent.CollapseBehavior.Show);
                i++;
                contents[i] = new SerializedContent(property.FindPropertyRelative("numberScale"), "Number Scale", SerializedContent.CollapseBehavior.Hide);
                i++;
                break;
            case 3:
                contents[i] = new SerializedContent(property.FindPropertyRelative("baseValue"), "Base Value", SerializedContent.CollapseBehavior.Show);
                i++;
                contents[i] = new SerializedContent(property.FindPropertyRelative("modifier"), "Absolute Variance", SerializedContent.CollapseBehavior.Show);
                i++;
                break;
        }

        contents[i] = new SerializedContent(randomType, "Random Type", SerializedContent.CollapseBehavior.AlwaysInHeader);

        return contents;
    }

    public int VisibleFields(SerializedProperty property)
    {
        int fields = 0;
        switch (property.FindPropertyRelative("variableType").enumValueIndex)
        {
            case 0:
                fields = 2;
                break;
            case 1:
                fields = 3;
                break;
            case 2:
                fields = 4;
                break;
            case 3:
                fields = 3;
                break;
        }
        return fields;
    }

}


[CustomPropertyDrawer(typeof(RandomizedVector3))]
public class RandomizedVector3Editor : CollapsibleDrawer
{
       
    
    public override SerializedContent[] GetContents(SerializedProperty property)
    {
        SerializedProperty randomType = property.FindPropertyRelative("variableType");
        SerializedProperty referenceFrame = property.FindPropertyRelative("referenceFrame");

        int length = GetLength(randomType.enumValueIndex, referenceFrame.enumValueIndex);

        SerializedContent[] contents = new SerializedContent[length];

        contents[0] = new SerializedContent(referenceFrame, "Reference Frame", SerializedContent.CollapseBehavior.AlwaysInHeader);
        contents[1] = new SerializedContent(randomType, "Randomization Type", SerializedContent.CollapseBehavior.AlwaysInHeader);

        int i = 2;

        switch (referenceFrame.enumValueIndex)
        {
            case 0:
                break;
            case 1:
                contents[i] = new SerializedContent(property.FindPropertyRelative("center"), "Center", SerializedContent.CollapseBehavior.Hide);
                i++;
                break;
        }

        switch (randomType.enumValueIndex)
        {
            case 0:
                break;
            case 1:
                contents[i] = new SerializedContent(property.FindPropertyRelative("distance"), "Distance", SerializedContent.CollapseBehavior.Hide);
                i++;
                break;
            case 2:
                contents[i] = new SerializedContent(property.FindPropertyRelative("bound1"), "Range", SerializedContent.CollapseBehavior.Hide);
                i++;
                break;
            case 3:
                contents[i] = new SerializedContent(property.FindPropertyRelative("bound1"), "Minimum", SerializedContent.CollapseBehavior.Hide);
                i++;
                contents[i] = new SerializedContent(property.FindPropertyRelative("bound2"), "Maximum", SerializedContent.CollapseBehavior.Hide);
                i++;
                break;
        }

        return contents;
    }
    
    public int GetLength(int variableType, int referenceFrame)
    {
        int lines = 2;

        switch (variableType)
        {
            case 0:
                break;
            case 1:
                lines++;
                break;
            case 2:
                lines++;
                break;
            case 3:
                lines += 2;
                break;
        }

        if (referenceFrame != 0)
        {
            lines++;
        }

        return lines;
    }

}



