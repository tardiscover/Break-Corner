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

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //This locks the RigidBody so that it does not move or rotate in the Z axis.
        //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Corner"))
        {
            //Debug.Log("OnCollisionEnter " + m_Rigidbody.velocity.ToString());   //!!!!!!!!!
            ToggleFrameWithin();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //Debug.Log("OnCollisionExit " + m_Rigidbody.velocity.ToString());   //!!!!!!!!!
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * speedIncrease;
        
        //Check if we are going nearly horizontally or nearly vertically, as both are a problem
        if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) < nearHorizontalCutoff)
        {
            //Going almost horizontally, so add a little vertical force.
            Debug.Log("Horiz, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
            velocity += velocity.y > 0 ? Vector3.up * vertVelocityNudge : Vector3.down * vertVelocityNudge;
        }
        else if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) > nearVerticalCutoff)
        {
            //Going almost vertically
            Debug.Log("Vert, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ball OnTriggerEnter");   //!!!!!!!!!

        //About to collide with corner, so unlock constraints
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ball OnTriggerExit");   //!!!!!!!!!

        if (other.CompareTag("CornerProximityDetector"))
        {
            if (frameWithin == FrameWithin.Right)
            {
                //This ensures Z is 0 and locks the RigidBody so that it does not move or rotate in the Z axis.
                if (m_Rigidbody.position.z != 0)
                {
                    m_Rigidbody.position = new Vector3(m_Rigidbody.position.x, m_Rigidbody.position.y, 0.0f);
                }
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                //This ensures X is 0 and locks the RigidBody so that it does not move or rotate in the X axis.
                if (m_Rigidbody.position.x != 0)
                {
                    m_Rigidbody.position = new Vector3(0.0f, m_Rigidbody.position.y, m_Rigidbody.position.z);
                }
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            }
        }
    }
}
