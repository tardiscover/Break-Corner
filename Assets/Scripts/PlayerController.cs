using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private Rigidbody rb;
    private float movementRightPaddle;    //X based on input device, not screen
    private float movementLeftPaddle;    //Y based on input device, not screen
    public float speed = 2.0f;
    public float minPosition = 0.3f;
    public float maxPosition = 4.5f;
    public Transform leftPaddleTransform;

    protected Vector3 startingPosition;
    protected Vector3 leftPaddleStartingPosition;


    protected void Awake()
    {
        startingPosition = transform.position;
        leftPaddleStartingPosition = leftPaddleTransform.position;
    }

    private void OnMoveLeftPaddle(InputValue movementValue)
    {
        movementLeftPaddle = movementValue.Get<float>();
    }

    private void OnMoveRightPaddle(InputValue movementValue)
    {
        movementRightPaddle = movementValue.Get<float>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (movementRightPaddle != 0)
        {
            //Debug.Log("right move: " + input);

            Vector3 pos = transform.position;
            pos.x += movementRightPaddle * speed * Time.deltaTime;

            if (pos.x > maxPosition)
                pos.x = maxPosition;
            else if (pos.x < minPosition)
                pos.x = minPosition;

            transform.position = pos;
        }

        if (movementLeftPaddle != 0)
        {
            //Debug.Log("left move: " + input);

            Vector3 pos = leftPaddleTransform.position;
            pos.z += movementLeftPaddle * speed * Time.deltaTime;
            //Note: signs are reversed since Z axis in opposite direction of game's conceptional z axis.
            if (pos.z < -maxPosition)
                pos.z = -maxPosition;
            else if (pos.z > -minPosition)
                pos.z = -minPosition;

            leftPaddleTransform.position = pos;
        }

    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
        leftPaddleTransform.position = leftPaddleStartingPosition;
    }
}
