using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BallValues
{
    public BallValues(GameObject ball)
    {
        this.ball = ball;
        distance = 0;
        sr = ball.GetComponent<SpriteRenderer>();
    }
    
    public GameObject ball;
    public float distance;
    public SpriteRenderer sr;
}
