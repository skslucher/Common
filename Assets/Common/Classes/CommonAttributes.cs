using UnityEngine;
using System;
using System.Collections;


public class ReplaceDisplayPropertyAttribute : PropertyAttribute
{
    public string relativePropertyPath;
    public ReplaceDisplayPropertyAttribute(string relativePropertyPath)
    {
        this.relativePropertyPath = relativePropertyPath;
    }
}

public enum CustomDisplayMode
{
    NoLabel,
    LabelAsHeader,
    NoDropdown
}

public class CustomDisplayAttribute : PropertyAttribute
{
    public CustomDisplayMode displayMode;

    public CustomDisplayAttribute(CustomDisplayMode displayMode = CustomDisplayMode.NoLabel)
    {
        this.displayMode = displayMode;
    }
}



public class TagAttribute : PropertyAttribute
{

}

