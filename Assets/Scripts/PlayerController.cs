using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private Rigidbody rb;
    private float movementX;    //X based on input device, not screen
    private float movementY;    //Y based on input device, not screen
    public float speed = 2.0f;
    public float minPosition = 0.3f;
    public float maxPosition = 4.5f;
    public Transform leftPaddleTransform;

    protected Vector3 startingPosition;
    protected Vector3 leftPaddleStartingPosition;

    //Below are used to figure out what the joystick position is relative to the "/" and "\" diagonals.
    private Vector2 slashVector = new Vector2(1, 1).normalized;
    private Vector2 backslashVector = new Vector2(1, -1).normalized;

    protected void Awake()
    {
        startingPosition = transform.position;
        leftPaddleStartingPosition = leftPaddleTransform.position;
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnMoveLeftStick(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        //Figure out where the joystick is when projected along the "/" diagonal.
        movementY = Vector2.Dot(movementVector, slashVector);

        Debug.Log("movementVector: " + movementVector.ToString() + ", slashVector: " + slashVector.ToString() + ", movementY: " + movementY);   //!!!!!!!!!!!!!!!
    }

    private void OnMoveRightStick(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        //Figure out where the joystick is when projected along the "\" diagonal.
        movementX = Vector2.Dot(movementVector, backslashVector);

        Debug.Log("movementVector: " + movementVector.ToString() + ", backslashVector: " + backslashVector.ToString() + ", movementX: " + movementX);   //!!!!!!!!!!!!!!!
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (movementX != 0)
        {
            //Debug.Log("right move: " + input);

            Vector3 pos = transform.position;
            pos.x += movementX * speed * Time.deltaTime;

            if (pos.x > maxPosition)
                pos.x = maxPosition;
            else if (pos.x < minPosition)
                pos.x = minPosition;

            transform.position = pos;
        }

        if (movementY != 0)
        {
            //Debug.Log("left move: " + input);

            Vector3 pos = leftPaddleTransform.position;
            pos.z += movementY * speed * Time.deltaTime;
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
