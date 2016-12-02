using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AxisButtons : Singleton<AxisButtons> {

    public class Axis
    {
        const float deadzone = .5f;

        string axisName;

        float oldValue;
        float currentValue;

        public Axis(string axisName)
        {
            this.axisName = axisName;
            oldValue = 0f;
            currentValue = 0f;
        }

        public void Update()
        {
            oldValue = currentValue;
            currentValue = Input.GetAxisRaw(axisName);
        }

        public bool GetAxisDown(bool isPositive = true)
        {
            if (isPositive)
            {
                return (oldValue < deadzone && currentValue >= deadzone);
            }
            else
            {
                return (oldValue > -deadzone && currentValue <= -deadzone);
            }
        }
    }


    public static readonly string[] axisNames =  {  "Horizontal", "Vertical"  };


    static Dictionary<string, Axis> axes;
    

    static public bool GetAxisDown(string axisName, bool isPositive = true)
    {
        if (Instance == null) return false;

        if (axes.ContainsKey(axisName))
        {
            return axes[axisName].GetAxisDown(isPositive);
        }
        else
        {
            Debug.Log("Error: Axis \"" + axisName + "\" not found");
            return false;
        }
    }





    public void Awake()
    {
        axes = new Dictionary<string, Axis>();
        for (int i = 0; i < axisNames.Length; i++) axes.Add(axisNames[i], new Axis(axisNames[i]));
    }
    
    public void LateUpdate()
    {
        foreach(Axis axis in axes.Values)
        {
            axis.Update();
        }
    }


}
