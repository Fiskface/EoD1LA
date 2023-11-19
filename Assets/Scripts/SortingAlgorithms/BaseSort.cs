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
            writer.Write(this.name+";");
            for (var i = 0; i < averageTimeList.Count; i++)
            {
                var temp = Decimal.Parse(averageTimeList[i].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                writer.Write(temp + ";");
            }
        }
    }
}
