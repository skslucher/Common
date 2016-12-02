using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PerformanceTest : MonoBehaviour
{

    public delegate float TestAction(float t);

    public class TestCase
    {
        public static float[] randomValues;
        public static void GenerateRandomValues(int iterations)
        {
            randomValues = new float[iterations];
            for (int i = 0; i < randomValues.Length; i++) randomValues[i] = UnityEngine.Random.value;
        }


        public string label;
        public TestAction action;

        public float time;

        public TestCase(string label, TestAction action)
        {
            this.label = label;
            this.action = action;

            time = 0f;
        }
        
        public void TestPerformance(int iterations, float control = 0f)
        {
            if (randomValues == null) GenerateRandomValues(iterations);

            int i;
            float t = 1f;
            
            System.DateTime startTime = System.DateTime.Now;
            
            for(i=0; i<iterations; i++)
            {
                t = action(randomValues[i]);
            }

            time = (float)System.DateTime.Now.Subtract(startTime).TotalMilliseconds;


            string resultMsg = "Test case: \"" + label + "\": " + time.ToString("0") + "ms";
            if (control != 0) resultMsg += " (" + (time - control).ToString("0") + "ms plus control)";

            Debug.Log(resultMsg);
        }
    }
    private List<TestCase> testCases;
    

    const int iterations = 1000000;
    
    [BitStrap.Button]
    public void RunPerformanceTest()
    {
        Debug.Log(Mathf.Repeat(5f, 1f));
        Debug.Log(Mathf.Repeat(4.9f, 1f));
        Debug.Log(Mathf.Repeat(-5f, 1f));
        Debug.Log(Mathf.Repeat(-4.9f, 1f));
        
        InitializeCases();

        TestAllCases();
    }
    

    public void InitializeCases()
    {
        testCases = new List<TestCase>();

        testCases.Add(new TestCase("Control", t => Control(t)));
        
        testCases.Add(new TestCase("Com.Normalize", t => Com.Normalize(t - .5f)));
        
        testCases.Add(new TestCase("MathTest.Normalize", t => MathTest.Normalize(t - .5f)));
    }


    public void TestAllCases()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log("Running " + iterations + " iterations of each...");

        for (int i = 0; i < testCases.Count; i++)
        {
            testCases[i].TestPerformance(iterations, testCases[0].time);
        }
    }
    

    public static float Control(float t)
    {
        return t;
    }
    

}

public class MathTest
{

    static public int Normalize(float value)
    {
        if (value > 0f) return 1;
        if (value < 0f) return -1;
        return 0;
    }

    public static float Pow1(float f, int pow)
    {
        float returnValue = 1f;
        for (int i = 0; i < pow; i++)
        {
            returnValue *= f;
        }
        return returnValue;
    }

    public static float Pow2(float f, int pow)
    {
        float returnValue = 1f;
        for (int i = 0; i < pow; i++) returnValue *= f;
        return returnValue;
    }

}