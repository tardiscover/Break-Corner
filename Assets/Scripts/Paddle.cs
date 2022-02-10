using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 2.0f;

    public float MinPosition = 0.3f;
    public float MaxPosition = 4.5f;

    protected Vector3 startingPosition;

    protected void Awake()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            //Debug.Log("right move: " + input);

            Vector3 pos = transform.position;
            pos.x += input * Speed * Time.deltaTime;

            if (pos.x > MaxPosition)
                pos.x = MaxPosition;
            else if (pos.x < MinPosition)
                pos.x = MinPosition;

            transform.position = pos;
        }
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
