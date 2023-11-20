using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Algorithm Port", menuName = "Scriptable objects/algorithmport")]
public class AlgorithmPortSO : ScriptableObject
{
    public UnityAction SignalSort = delegate {  };
    public UnityAction SignalIntervalIncrease = delegate {  };
    public float MaxTimePerInterval = 10;
    public string path;
}
