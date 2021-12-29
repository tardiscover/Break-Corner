using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;

    private float initialPos;

    private void Start()
    {
        initialPos = transform.position.x;
    }

    // Update is called once per frame
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

            //if (pos.x > MaxMovement)
            //    pos.x = MaxMovement;
            //else if (pos.x < -MaxMovement)
            //    pos.x = -MaxMovement;
            if (pos.x > initialPos + MaxMovement)
                pos.x = initialPos + MaxMovement;
            else if (pos.x < initialPos - MaxMovement)
                pos.x = initialPos - MaxMovement;

            transform.position = pos;
        }
    }
}
