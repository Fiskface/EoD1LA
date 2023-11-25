using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public Color sortedColor = Color.blue;
    public Color unsortedColor = Color.red;
    public int maxBalls = 10000;
    private int ballsPerInterval = 100;
    public int samplesPerInterval = 100;
    public float maxAverageTimePerInterval = 0.5f;
    public List<int> increaseBPIAt;
    public List<int> increaseBPITo;
    [Header("Runtime")] public int currentBalls = 0;
    
    private int frameCounter = 0;
    private List<int> antalBollarList = new List<int>();

    void Awake()
    {
        algorithmPort.MaxTimePerInterval = maxAverageTimePerInterval * samplesPerInterval;
        whiteBall = Instantiate(whiteBallSpawn);
        
        ballArray.array = new BallValues[ballsPerInterval];
        for (int i = 0; i < ballsPerInterval; i++)
        {
            SpawnBall(i);
        }

        ballArray.sortedArray = (BallValues[])ballArray.array.Clone();
        currentBalls = ballArray.array.Length;
        antalBollarList.Add(currentBalls);
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCounter >= samplesPerInterval)
        {
            if (ballArray.array.Length < maxBalls)
            {
                IncreaseInterval();
                frameCounter = 0;
            }
            else
            {
                Application.Quit();
                //UnityEditor.EditorApplication.isPlaying = false;
            }
            
        }
        
        UpdateBallValues();
        
        Profiler.BeginSample("Sorting", this);
        algorithmPort.SignalSort();
        Profiler.EndSample();
        
        SetColors();
        
        frameCounter++;
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
            a[j].sr.color = unsortedColor;
        }
        
        for (int j = 0; j < a.Length / 4; j++)
        {
            a[j].sr.color = sortedColor;
        }
        Profiler.EndSample();
    }

    private void IncreaseInterval()
    {
        if(increaseBPIAt.Any())
        {
            if (currentBalls >= increaseBPIAt[0])
            {
                ballsPerInterval = increaseBPITo[0];
                increaseBPITo.RemoveAt(0);
                increaseBPIAt.RemoveAt(0);
            }
        }
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
        string path = algorithmPort.path;
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


