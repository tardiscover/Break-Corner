using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : Paddle
{
    private float initialPos;

    private void Start()
    {
        initialPos = transform.position.z;
    }

    public override void Move()
    {
        float input = Input.GetAxis("Vertical");

        if (input != 0)
        {
            //Debug.Log("left move: " + input);

            Vector3 pos = transform.position;
            pos.z += input * Speed * Time.deltaTime;

            if (pos.z > initialPos + MaxMovement)
                pos.z = initialPos + MaxMovement;
            else if (pos.z < initialPos - MaxMovement)
                pos.z = initialPos - MaxMovement;

            transform.position = pos;
        }

    }
}
