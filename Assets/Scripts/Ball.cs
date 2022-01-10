using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrameWithin
{
    Left,
    Right
}

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public FrameWithin frameWithin = FrameWithin.Right;

    //Amounts to push vertically or horizontally if movement too horizontal or to vertical
    private float vertVelocityNudge = 0.5f;
    private float horizVelocityNudge = 0.25f;

    //When doing a dot product with Vector3.up, point at which near vertical or near horizontal
    private float nearVerticalCutoff = 0.97f;
    private float nearHorizontalCutoff = 0.1f;

    private float speedIncrease = 0.01f;
    private float maxVelocity = 3.0f;

    private MainManager mainManager;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        //This locks the RigidBody so that it does not move or rotate in the Z axis.
        ConstrainZTo(0.0f);

        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();

    }

    void ToggleFrameWithin()
    {
        if (frameWithin == FrameWithin.Right)
        {
            frameWithin = FrameWithin.Left;
        }
        else
        {
            frameWithin = FrameWithin.Right;
       }
    }

    //Adjust Velocity after a collision
    void AdjustVelocity()
    {
        var velocity = m_Rigidbody.velocity;

        //after a collision we accelerate a bit
        velocity += velocity.normalized * speedIncrease;

        //Check if we are going nearly horizontally or nearly vertically, as both are a problem
        if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) < nearHorizontalCutoff)
        {
            //Going almost horizontally, so add a little vertical force.
            //Debug.Log("Horiz, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
            velocity += velocity.y > 0 ? Vector3.up * vertVelocityNudge : Vector3.down * vertVelocityNudge;
        }
        else if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) > nearVerticalCutoff)
        {
            //Going almost vertically
            //Debug.Log("Vert, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
            if (frameWithin == FrameWithin.Right)
            {
                velocity += velocity.x > 0 ? Vector3.right * horizVelocityNudge : Vector3.left * horizVelocityNudge;
            }
            else
            {
                velocity += velocity.z > 0 ? Vector3.back * horizVelocityNudge : Vector3.forward * horizVelocityNudge;
            }
        }

        //max velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        //Debug.Log(frameWithin.ToString() + ", " + m_Rigidbody.position.ToString() + ", " + velocity.ToString()); //!!!!!!!!!!!!!!

        m_Rigidbody.velocity = velocity;
    }

    private void RemoveConstraints()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void ConstrainXTo(float value)
    {
        //This ensures X is value passsed and locks the RigidBody so that it does not move on the X axis.
        //(And allows movement on other axes.)
        if (m_Rigidbody.position.x != value)
        {
            m_Rigidbody.position = new Vector3(value, m_Rigidbody.position.y, m_Rigidbody.position.z);
        }
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
    }

    private void ConstrainZTo(float value)
    {
        //This ensures Z is value passed and locks the RigidBody so that it does not move on the Z axis.
        //(And allows movement on other axes.)
        if (m_Rigidbody.position.z != value)
        {
            m_Rigidbody.position = new Vector3(m_Rigidbody.position.x, m_Rigidbody.position.y, value);
        }
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    // When ball approaches corner, it should trigger the following events in order.
    // (
    //  OnTriggerEnter with CornerProximityDetector
    //  OnCollisionEnter with Corner
    //  OnCollisionExit with Corner
    //  OnTriggerExit with CornerProximityDetector
    // )
    // OnCollisionEnter and OnCollisionExit will also happen with borders (walls and ceiling and DeathZone (floors))

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CornerProximityDetector"))
        {
            //About to collide with corner, so unlock constraints
            RemoveConstraints();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Corner"))
        {
            ToggleFrameWithin();
        }
        else if (other.gameObject.CompareTag("Paddle"))
        {
            if (mainManager.bricksLeft <= 0)
            {
                if (mainManager.lineCount < mainManager.maxLineCount)
                {
                    mainManager.lineCount++;
                }
                mainManager.InitBricks();
            }
        }
    }

    //private void OnCollisionExit(Collision other)
    private void OnCollisionExit()  //Saves some calculations to skip parameter if it won't be used.
    {
        //Debug.Log("OnCollisionExit " + m_Rigidbody.velocity.ToString());   //!!!!!!!!!
        AdjustVelocity();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CornerProximityDetector"))
        {
            if (frameWithin == FrameWithin.Right)
            {
                ConstrainZTo(0.0f);
            }
            else
            {
                ConstrainXTo(0.0f);
            }
        }
    }
}
