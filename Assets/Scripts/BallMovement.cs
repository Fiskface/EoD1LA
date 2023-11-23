using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallMovement : MonoBehaviour
{
    private Vector3 direction;
    public float speed = 2;
    private float oneOverSixty = 1f / 60f;
    
    void Awake()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
    }

    private void Update()
    {
        transform.position += speed * oneOverSixty * direction;
        if (Math.Abs(transform.position.x) > 8.8f)
        {
            transform.position = new Vector3(Math.Sign(transform.position.x) * 8.8f, transform.position.y);
            direction.x = -direction.x;
        }
        if (Math.Abs(transform.position.y) > 5f)
        {
            transform.position = new Vector3(transform.position.x, Math.Sign(transform.position.y) * 5f);
            direction.y = -direction.y;
        }
    }
}
