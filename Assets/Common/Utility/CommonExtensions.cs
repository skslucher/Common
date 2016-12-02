using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CommonExtensions {


    //http://wiki.unity3d.com/index.php/GetOrAddComponent

    /// <summary>
    /// Gets or add a component. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    static public T GetOrAddComponent<T>(this Component child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }



    static public void Zero(this Transform transform, bool local = true, bool scale = false)
    {
        if (local)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if (scale)
                transform.localScale = Vector3.one;
        }
        else
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            if (scale)
                transform.SetGlobalScale(Vector3.one);
        }
    }

    static public void Match(this Transform transform, Transform toMatch, bool matchScale = false)
    {
        transform.position = toMatch.position;
        transform.rotation = toMatch.rotation;

        if (matchScale)
        {
            transform.SetGlobalScale(toMatch.lossyScale);
        }
    }

    static public void SetGlobalScale(this Transform transform, Vector3 scale)
    {
        if (transform.parent == null)
        {
            transform.localScale = scale;
        }
        else
        {
            transform.localScale = Vector3.one;
            var m = transform.worldToLocalMatrix;
            m.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            transform.localScale = m.MultiplyPoint(scale);
        }
    }



    /// <summary>
    /// Check to see if a flags enumeration has a specific flag set.
    /// </summary>
    /// <param name="variable">Flags enumeration to check</param>
    /// <param name="value">Flag to check for</param>
    /// <returns></returns>
    public static bool HasFlag(this System.Enum variable, System.Enum value)
    {
        if (variable == null)
            return false;

        //if (value == null)
        //    throw new ArgumentNullException("value");

        // Not as good as the .NET 4 version of this function, but should be good enough
        //if (!System.Enum.IsDefined(variable.GetType(), value))
        //{
        //    throw new ArgumentException(string.Format(
        //        "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
        //        value.GetType(), variable.GetType()));
        //}

        ulong num = System.Convert.ToUInt64(value);
        return ((System.Convert.ToUInt64(variable) & num) == num);

    }


    public static T RandomValue<T>(this T[] array)
    {
        if (array == null || array.Length == 0) return default(T);

        return array[RandUtil.Int(array.Length)];
    }



    static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        TValue returnValue;
        if (!dictionary.TryGetValue(key, out returnValue))
        {
            dictionary.Add(key, returnValue = new TValue());
        }
        return returnValue;
    }

    static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, System.Func<TKey, TValue> initFunc)
    {
        TValue returnValue;
        if (!dictionary.TryGetValue(key, out returnValue))
        {
            dictionary.Add(key, returnValue = initFunc(key));
        }
        return returnValue;
    }
}
