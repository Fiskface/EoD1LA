using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class ArraySort : BaseSort
{
    protected override void Sort()
    {
        StartOfSort();
        
        Array.Sort(sortedBalls, (o1, o2) => o1.distance.CompareTo(o2.distance));
        
        EndOfSort();
    }
}
