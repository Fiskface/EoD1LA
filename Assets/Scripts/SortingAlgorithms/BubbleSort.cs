using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class BubbleSort : BaseSort
{
    protected override void Sort()
    {
        StartOfSort();
        
        int n = sortedBalls.Length;
        bool swapped = n > 1;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if(sortedBalls[i-1].distance > sortedBalls[i].distance)
                {
                    (sortedBalls[i-1], sortedBalls[i]) = (sortedBalls[i], sortedBalls[i-1]);
                    swapped = true;
                }
            }

            n--;
        }
        
        EndOfSort();
    }
}
