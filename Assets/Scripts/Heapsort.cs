using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Heapsort : MonoBehaviour, IAlgorithm
{
    public AlgorithmPortSO algorithmPort;
    //public GameObjectSO whiteBall;
    void Start()
    {
        algorithmPort.addAlgorithm(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Heapsort
    public BallValues[] Sort(BallValues[] array)
    {
        Profiler.BeginSample("Heapsort", this);
        int n = array.Length;

        if (n <= 1)
        {
            return array;
        }
        
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(array, n, i);
        }

        for (int i = n - 1; i >= 0; i--)
        {
            Swap(array, 0, i);
            Heapify(array, i, 0);
        }
        Profiler.EndSample();
        return array;
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
