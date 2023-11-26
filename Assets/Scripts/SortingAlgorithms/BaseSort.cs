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

    protected BallValues[] ballsToSort;
    public ArrayOfBalls ballArray;

    protected List<float> timeList = new List<float>();
    private List<float> averageTimeList = new List<float>();
    private List<float> medianTimeList = new List<float>();

    private float temp;

    protected float timeAccumulated = 0f;
    
    private void OnEnable()
    {
        algorithmPort.SignalSort += Sort;
        algorithmPort.SignalIntervalIncrease += MakeAverage;
        algorithmPort.amountOfSorters++;
    }
    private void OnDisable()
    {
        algorithmPort.SignalSort -= Sort;
        algorithmPort.SignalIntervalIncrease -= MakeAverage;
        if (timeList.Any()) MakeAverage();
        WriteToFile();
        algorithmPort.RemoveSorter();
    }

    protected virtual void Sort()
    {
        
    }

    protected void StartOfSort()
    {
        ballsToSort = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample(GetType().Name, this);
        temp = Time.realtimeSinceStartup;
    }
    
    protected void EndOfSort()
    {
        temp = Time.realtimeSinceStartup - temp;
        Profiler.EndSample();
        timeAccumulated += temp;
        timeList.Add(temp * 1000);
        ballArray.sortedArray = ballsToSort;
        
        if(timeAccumulated > algorithmPort.MaxTimePerInterval) gameObject.SetActive(false);
    }
    
    private void MakeAverage()
    {
        averageTimeList.Add(timeList.Average());
        timeList.Sort();
        medianTimeList.Add(timeList[timeList.Count / 2]);
        
        timeList.Clear();
        timeAccumulated = 0;
    }

    private void WriteToFile()
    {
        string path = algorithmPort.path + GetType() + ".txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(GetType()+";");
            for (var i = 0; i < averageTimeList.Count; i++)
            {
                var temp = Decimal.Parse(averageTimeList[i].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                writer.Write(temp + ";");
            }
        }
        path = algorithmPort.path + GetType() + "Median.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(GetType()+"Median;");
            for (var i = 0; i < medianTimeList.Count; i++)
            {
                var temp = Decimal.Parse(medianTimeList[i].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                writer.Write(temp + ";");
            }
        }
    }
}
