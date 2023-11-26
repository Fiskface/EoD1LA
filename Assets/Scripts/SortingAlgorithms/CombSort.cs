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

        int n = ballsToSort.Length;
        int gap = n;
        bool swapped = true;
        while (gap != 1 || swapped)
        {
            gap = getNextGap(gap);
            swapped = false;

            for (int i = 0; i < n - gap; i++)
            {
                if(ballsToSort[i].distance > ballsToSort[i+gap].distance)
                {
                    (ballsToSort[i], ballsToSort[i + gap]) = (ballsToSort[i + gap], ballsToSort[i]);
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
