using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
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
    public int intervalBalls = 100;
    public int framesPerInterval = 100;
    
    private int frameCounter = 0;
    private int currentBallCount;

    void Awake()
    {
        whiteBall = Instantiate(whiteBallSpawn);
        currentBallCount = minBalls;
        ballArray.array = new BallValues[minBalls];
        for (int i = 0; i < minBalls; i++)
        {
            SpawnBall(i);
        }

        ballArray.sortedArray = (BallValues[])ballArray.array.Clone();
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
        if (frameCounter >= framesPerInterval)
        {
            //SwapInterval
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
}


