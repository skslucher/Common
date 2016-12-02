using UnityEngine;
using System.Collections;


public class GenericListWrapper { }

public class ListWrapper<TObj> : GenericListWrapper
{
    public TObj[] list;

    public TObj this[int index]
    {
        get { return list[index]; }
        set { list[index] = value; }
    }

    public static implicit operator ListWrapper<TObj>(TObj[] list)
    {
        ListWrapper<TObj> newListWrapper = new ListWrapper<TObj>();
        newListWrapper.list = list;
        return newListWrapper;
    }
}


[System.AttributeUsage(System.AttributeTargets.Field)]
public class ListAdvancedAttribute : PropertyAttribute //Only apply to closed-typed ListWrapper fields please
{
    public bool isFixedLength;
    public int length;
    public string[] labels;

    public ListAdvancedAttribute()
    {
        isFixedLength = false;
    }

    public ListAdvancedAttribute(int length)
    {
        isFixedLength = true;
        this.length = length;
    }

    public ListAdvancedAttribute(System.Type enumType)
    {
        if (!enumType.IsEnum)
        {
            Debug.Log("Error: " + enumType + " is not an enum.");
            return;
        }

        labels = System.Enum.GetNames(enumType);
        length = labels.Length;
        isFixedLength = true;
    }

    public ListAdvancedAttribute(string[] labels)
    {
        this.labels = labels;
        length = labels.Length;
        isFixedLength = true;
    }
}


