using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Heapsort : MonoBehaviour
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
            Swap(sortedBalls, 0, i);
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
            Swap(array, i, max);
            Heapify(array, size, max);
        }
    }

    private void Swap(BallValues[] array, int i1, int i2)
    {
        (array[i1], array[i2]) = (array[i2], array[i1]);
    }
}
