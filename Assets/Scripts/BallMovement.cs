using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallMovement : MonoBehaviour
{
    private Vector3 direction;
    public float speed = 2;
    
    void Awake()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
    }

    private void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * direction;
        if (Mathf.Abs(transform.position.x) > 8.8f)
        {
            transform.position = new Vector3(Mathf.Sign(transform.position.x) * 8.8f, transform.position.y);
            direction.x = -direction.x;
        }
        if (Mathf.Abs(transform.position.y) > 5f)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sign(transform.position.y) * 5f);
            direction.y = -direction.y;
        }
    }
}
