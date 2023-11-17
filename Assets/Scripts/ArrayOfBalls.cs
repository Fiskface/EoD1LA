using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "array", menuName = "Scriptable objects/array")]
public class ArrayOfBalls : ScriptableObject
{
    public BallValues[] array;
    public BallValues[] sortedArray;
}
