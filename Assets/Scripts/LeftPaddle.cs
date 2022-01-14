using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : Paddle    // INHERITANCE
{
    public override void Move()     // POLYMORPHISM
    {
        float input = Input.GetAxis("Vertical");

        if (input != 0)
        {
            //Debug.Log("left move: " + input);

            Vector3 pos = transform.position;
            pos.z += input * Speed * Time.deltaTime;
            //Note: signs are reversed since Z axis in opposite diection of game's conceptional z axis.
            if (pos.z < -MaxPosition)
                pos.z = -MaxPosition;
            else if (pos.z > -MinPosition)
                pos.z = -MinPosition;

            transform.position = pos;
        }
    }
}
