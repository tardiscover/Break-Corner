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
