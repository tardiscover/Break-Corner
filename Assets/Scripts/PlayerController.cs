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

    private float leftPaddleLeftButtonIsPressed = 0.0f;
    private float leftPaddleRightButtonIsPressed = 0.0f;
    private float rightPaddleLeftButtonIsPressed = 0.0f;
    private float rightPaddleRightButtonIsPressed = 0.0f;

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

    // Mouse/Touch events for Left Paddle

    public void OnLeftPaddleLeftButtonDown()
    {
        leftPaddleLeftButtonIsPressed = -1.0f;
        SetMovementLeftPaddle();
    }

    public void OnLeftPaddleLeftButtonUp()
    {
        leftPaddleLeftButtonIsPressed = 0.0f;
        SetMovementLeftPaddle();
    }

    public void OnLeftPaddleRightButtonDown()
    {
        leftPaddleRightButtonIsPressed = 1.0f;
        SetMovementLeftPaddle();
    }

    public void OnLeftPaddleRightButtonUp()
    {
        leftPaddleRightButtonIsPressed = 0.0f;
        SetMovementLeftPaddle();
    }

    private void SetMovementLeftPaddle()
    {
        movementLeftPaddle = leftPaddleLeftButtonIsPressed + leftPaddleRightButtonIsPressed;
    }

    // Mouse/Touch events for Right Paddle

    public void OnRightPaddleLeftButtonDown()
    {
        rightPaddleLeftButtonIsPressed = -1.0f;
        SetMovementRightPaddle();
    }

    public void OnRightPaddleLeftButtonUp()
    {
        rightPaddleLeftButtonIsPressed = 0.0f;
        SetMovementRightPaddle();
    }

    public void OnRightPaddleRightButtonDown()
    {
        rightPaddleRightButtonIsPressed = 1.0f;
        SetMovementRightPaddle();
    }

    public void OnRightPaddleRightButtonUp()
    {
        rightPaddleRightButtonIsPressed = 0.0f;
        SetMovementRightPaddle();
    }

    private void SetMovementRightPaddle()
    {
        movementRightPaddle = rightPaddleLeftButtonIsPressed + rightPaddleRightButtonIsPressed;
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
