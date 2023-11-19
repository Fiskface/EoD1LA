using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class CombSort : MonoBehaviour
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

    //Combsort
    public void Sort()
    {
        sortedBalls = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample("Heapsort", this);
        var temp = Time.realtimeSinceStartup;

        int n = sortedBalls.Length;
        int gap = n;
        bool swapped = true;
        while (gap != 1 || swapped)
        {
            gap = getNextGap(gap);
            swapped = false;

            for (int i = 0; i < n - gap; i++)
            {
                if(sortedBalls[i].distance > sortedBalls[i+gap].distance)
                {
                    (sortedBalls[i], sortedBalls[i + gap]) = (sortedBalls[i + gap], sortedBalls[i]);
                    swapped = true;
                }
            }
        }
        

        timeList.Add( Time.realtimeSinceStartup - temp);
        Profiler.EndSample();

        ballArray.sortedArray = sortedBalls;
    }

    private int getNextGap(int gap)
    {
        gap = (gap * 10) / 13;
        return gap < 1 ? 1 : gap;
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
        
        string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
        path = path + this.name + ".txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(name+";");
            for (var i = 0; i < averageTimeList.Count; i++)
            {
                var temp = Decimal.Parse(averageTimeList[i].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                writer.Write(temp + ";");
            }
        }
    }
}
