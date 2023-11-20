using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class BaseSort : MonoBehaviour
{
    public AlgorithmPortSO algorithmPort;

    protected BallValues[] sortedBalls;
    public ArrayOfBalls ballArray;

    protected List<float> timeList = new List<float>();
    private List<float> averageTimeList = new List<float>(){};

    private float temp;

    protected float timeAccumulated = 0f;
    
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

    protected virtual void Sort()
    {
        
    }

    protected void StartOfSort()
    {
        sortedBalls = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample(this.name, this);
        temp = Time.realtimeSinceStartup;
    }
    
    protected void EndOfSort()
    {
        temp = Time.realtimeSinceStartup - temp;
        Profiler.EndSample();
        timeList.Add(temp);
        timeAccumulated += temp;
        ballArray.sortedArray = sortedBalls;
        
        //TODO: Kill me here;
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
        timeAccumulated = 0;
    }

    private void WriteToFile()
    {
        string path = algorithmPort.path;
        path = path +this.name + ".txt";
        using (StreamWriter writer = new StreamWriter(path))
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
