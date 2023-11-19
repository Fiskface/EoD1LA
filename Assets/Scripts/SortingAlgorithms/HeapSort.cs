using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Profiling;

public class HeapSort : MonoBehaviour
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

    //Heapsort
    public void Sort()
    {
        sortedBalls = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample("Heapsort", this);
        var temp = Time.realtimeSinceStartup;
        int n = sortedBalls.Length;

        if (n <= 1)
        {
            return;
        }
        
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(sortedBalls, n, i);
        }

        for (int i = n - 1; i >= 0; i--)
        {
            (sortedBalls[0], sortedBalls[i]) = (sortedBalls[i], sortedBalls[0]);
            Heapify(sortedBalls, i, 0);
        }

        timeList.Add( Time.realtimeSinceStartup - temp);
        Profiler.EndSample();

        ballArray.sortedArray = sortedBalls;
    }

    private void Heapify(BallValues[] array, int size, int i)
    {
        var max = i;
        var leftChild = 2 * i + 1;
        var rightChild = 2 * i + 2;

        if (leftChild < size && array[max].distance < array[leftChild].distance) max = leftChild;

        if (rightChild < size && array[max].distance < array[rightChild].distance) max = rightChild;

        if (max != i)
        {
            (array[i], array[max]) = (array[max], array[i]);
            Heapify(array, size, max);
        }
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
        string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('A'));
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
