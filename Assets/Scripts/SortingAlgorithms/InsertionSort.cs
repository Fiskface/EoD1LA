using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : BaseSort
{
    protected override void Sort()
    {
        StartOfSort();

        BallValues x;
        int j;
        for (int i = 1; i < ballsToSort.Length; i++)
        {
            x = ballsToSort[i];
            for (j = i - 1; j >= 0 && ballsToSort[j].distance > x.distance; j--)
            {
                ballsToSort[j + 1] = ballsToSort[j];
            }

            ballsToSort[j + 1] = x;
        }
        
        EndOfSort();
    }
    
}
