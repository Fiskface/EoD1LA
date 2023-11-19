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
        //TODO: Fix inherit from BaseSort problem
        sortedBalls = (BallValues[])ballArray.array.Clone();
        Profiler.BeginSample("Array.sort", this);
        var temp = Time.realtimeSinceStartup;

        int n = sortedBalls.Length;
        bool swapped = n > 1;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if(sortedBalls[i-1].distance > sortedBalls[i].distance)
                {
                    (sortedBalls[i-1], sortedBalls[i]) = (sortedBalls[i-1], sortedBalls[i]);
                    swapped = true;
                }
            }

            n--;
        }
        
        timeList.Add( Time.realtimeSinceStartup - temp);
        Profiler.EndSample();
        ballArray.sortedArray = sortedBalls;
    }
}
