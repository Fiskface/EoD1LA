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
        
        int n = ballsToSort.Length;
        bool swapped = n > 1;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if(ballsToSort[i-1].distance > ballsToSort[i].distance)
                {
                    (ballsToSort[i-1], ballsToSort[i]) = (ballsToSort[i], ballsToSort[i-1]);
                    swapped = true;
                }
            }

            n--;
        }
        
        EndOfSort();
    }
}
