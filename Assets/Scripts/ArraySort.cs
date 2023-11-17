using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ArraySort : MonoBehaviour
{
    public AlgorithmPortSO algorithmPort;

    private BallValues[] sortedBalls;
    public ArrayOfBalls ballArray;

    private List<float> timeList = new List<float>();
    private void OnEnable()
    {
        algorithmPort.SignalSort += Sort;
    }
    private void OnDisable()
    {
        algorithmPort.SignalSort -= Sort;
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
}
