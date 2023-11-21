using System;
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
    public int amountOfSorters = 0;

    public void RemoveSorter()
    {
        amountOfSorters--;
        if (amountOfSorters == 0)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void OnEnable()
    {
        amountOfSorters = 0;
    }
}
