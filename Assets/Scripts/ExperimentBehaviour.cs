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
    
    [Header("Ball count")]
    public int Min = 100;
    public int Max = 10000;
    public int IntervalCount = 100;

    private List<IAlgorithm> sortAlgorithms = new List<IAlgorithm>();
    private int frameCounter;

    private void OnEnable()
    {
        algorithmPort.addAlgorithm += sortAlgorithms.Add;
    }
    
    private void OnDisable()
    {
        algorithmPort.addAlgorithm -= sortAlgorithms.Add;
    }

    void Awake()
    {
        whiteBall = Instantiate(whiteBallSpawn);
        ballArray.array = new BallValues[Min];
        for (int i = 0; i < Min; i++)
        {
            SpawnBall(i);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBallValues();
        
        Profiler.BeginSample("Algorithms", this);
        for (int i = 0; i < sortAlgorithms.Count; i++)
        {
            var temp = ballArray.array;
            var sortedTemp = sortAlgorithms[i].Sort(temp);
            
            for (int j = 0; j < sortedTemp.Length / 2; j++)
            {
                sortedTemp[j].sr.color = Color.gray;
            }
            
            for (int j = sortedTemp.Length / 2; j < sortedTemp.Length; j++)
            {
                sortedTemp[j].sr.color = Color.black;
            }
        }
        Profiler.EndSample();
        
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
            var ball = ballArray.array[i];
            ball.distance = Vector2.Distance(ball.ball.transform.position, whiteBall.transform.position);
        }
    }
}


