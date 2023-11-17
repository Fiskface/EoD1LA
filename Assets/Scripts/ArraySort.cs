using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class ArraySort : MonoBehaviour
{
    public AlgorithmPortSO algorithmPort;

    private BallValues[] sortedBalls;
    public ArrayOfBalls ballArray;

    private List<float> timeList = new List<float>();
    private List<float> averageTimeList = new List<float>(){};
    
    private void OnEnable()
    {
        algorithmPort.SignalSort += Sort;
        algorithmPort.SignalIntervalIncrease += MakeAverage;
    }
    private void OnDisable()
    {
        algorithmPort.SignalSort -= Sort;
        algorithmPort.SignalIntervalIncrease -= MakeAverage;
        if (timeList.Any()) MakeAverage();
        WriteToFile();
    }

    private void Sort()
    {
        sortedBalls = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample("Array.sort", this);
        var temp = Time.realtimeSinceStartup;
        Array.Sort(sortedBalls, (o1, o2) => o1.distance.CompareTo(o2.distance));
        timeList.Add( Time.realtimeSinceStartup - temp);
        Profiler.EndSample();
        ballArray.sortedArray = sortedBalls;
    }
    
    private void MakeAverage()
    {
        float temp = 0;
        foreach (var time in timeList)
        {
            temp += time;
        }
        
        temp /= timeList.Count;
        averageTimeList.Add(temp);
        
        timeList.Clear();
    }

    private void WriteToFile()
    {
        string fullPath = @"D:\Git\EoD1LA\"+this.name+".txt";
        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            writer.Write(this.name+";");
            for (var i = 0; i < averageTimeList.Count; i++)
            {
                var temp = Decimal.Parse(averageTimeList[i].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                writer.Write(temp + ";");
            }
        }
    }
}
