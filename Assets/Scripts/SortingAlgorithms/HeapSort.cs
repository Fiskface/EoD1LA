using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Profiling;

public class HeapSort : BaseSort
{
    //Heapsort
    protected override void Sort()
    {
        StartOfSort();
        
        int n = ballsToSort.Length;

        if (n <= 1)
        {
            return;
        }
        
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(ballsToSort, n, i);
        }

        for (int i = n - 1; i >= 0; i--)
        {
            (ballsToSort[0], ballsToSort[i]) = (ballsToSort[i], ballsToSort[0]);
            Heapify(ballsToSort, i, 0);
        }

        EndOfSort();
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
}
