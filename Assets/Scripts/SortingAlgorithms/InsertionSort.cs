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
        for (int i = 1; i < sortedBalls.Length; i++)
        {
            x = sortedBalls[i];
            for (j = i - 1; j >= 0 && sortedBalls[j].distance > x.distance; j--)
            {
                sortedBalls[j + 1] = sortedBalls[j];
            }

            sortedBalls[j + 1] = x;
        }
        
        EndOfSort();
    }
    
}
