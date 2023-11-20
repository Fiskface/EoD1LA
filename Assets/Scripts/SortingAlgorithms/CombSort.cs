using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class CombSort : BaseSort
{
    //Combsort
    protected override void Sort()
    {
        StartOfSort();

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

        EndOfSort();
    }

    private int getNextGap(int gap)
    {
        gap = (gap * 10) / 13;
        return gap < 1 ? 1 : gap;
    }
}
