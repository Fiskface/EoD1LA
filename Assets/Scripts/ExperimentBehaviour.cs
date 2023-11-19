using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ExperimentBehaviour : MonoBehaviour
{
    public AlgorithmPortSO algorithmPort;
    public ArrayOfBalls ballArray;
    public GameObject whiteBallSpawn;
    [NonSerialized] public GameObject whiteBall;
    public GameObject ballToSpawn;
    
    [Header("Simulation")]
    public int minBalls = 100;
    public int maxBalls = 10000;
    public int ballsPerInterval = 100;
    public int samplesPerInterval = 100;
    [Header("Runtime")] public int currentBalls = 0;
    
    private int frameCounter = 0;
    private List<int> antalBollarList = new List<int>();

    void Awake()
    {
        whiteBall = Instantiate(whiteBallSpawn);
        
        ballArray.array = new BallValues[minBalls];
        for (int i = 0; i < minBalls; i++)
        {
            SpawnBall(i);
        }

        ballArray.sortedArray = (BallValues[])ballArray.array.Clone();
        currentBalls = ballArray.array.Length;
        antalBollarList.Add(currentBalls);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBallValues();
        
        Profiler.BeginSample("Sorting", this);
        algorithmPort.SignalSort();
        Profiler.EndSample();
        
        SetColors();
        
        frameCounter++;
        if (frameCounter >= samplesPerInterval)
        {
            if (ballArray.array.Length < maxBalls)
            {
                IncreaseInterval();
                frameCounter = 0;
            }
            else
            {
                //TODO: End program here
            }
            
        }
        
    }

    private void SpawnBall(int arrayIndex)
    {
        var x = 8f;
        var y = 4.5f;
        Vector2 tempPos = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
        GameObject ball = Instantiate(ballToSpawn, tempPos, quaternion.identity);
        ballArray.array[arrayIndex] = new BallValues(ball);
    }

    private void UpdateBallValues()
    {
        for(int i = 0; i < ballArray.array.Length; i++)
        {
            ballArray.array[i].distance = Vector2.Distance(ballArray.array[i].ball.transform.position, whiteBall.transform.position);
        }
    }

    private void SetColors()
    {
        Profiler.BeginSample("Coloring", this);
        var a = ballArray.sortedArray;
        
        for (int j = a.Length / 4; j < a.Length; j++)
        {
            a[j].sr.color = Color.black;
        }
        
        for (int j = 0; j < a.Length / 4; j++)
        {
            a[j].sr.color = Color.gray;
        }
        Profiler.EndSample();
    }

    private void IncreaseInterval()
    {
        algorithmPort.SignalIntervalIncrease();
        
        Array.Resize(ref ballArray.array, ballArray.array.Length + ballsPerInterval);
        for (int i = 1; i <= ballsPerInterval; i++)
        {
            SpawnBall(ballArray.array.Length - i);
        }

        ballArray.sortedArray = (BallValues[])ballArray.array.Clone();
        currentBalls = ballArray.array.Length;
        antalBollarList.Add(currentBalls);
    }
    
    private void WriteToFile()
    {
        string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
        path = path + "AntalBollar.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write("Antal Bollar;");
            for (var i = 0; i < antalBollarList.Count; i++)
            {
                writer.Write(antalBollarList[i] + ";");
            }
        }
    }

    private void OnDisable()
    {
        WriteToFile();
    }
}


