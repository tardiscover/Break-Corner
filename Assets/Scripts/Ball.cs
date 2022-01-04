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

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
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
            ToggleFrameWithin();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //Check if we are going nearly horizontally or nearly vertically, as both are a problem
        if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) < 0.1f)
        {
            //Going almost horizontally, so add a little vertical force.
            Debug.Log("Horiz, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }
        else if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) > 0.97f)
        {
            //Going almost vertically
            Debug.Log("Vert, velocity.normalized=" + velocity.normalized.ToString() + ", Vector3.up=" + Vector3.up.ToString() + ", product=" + Vector3.Dot(velocity.normalized, Vector3.up).ToString()); //!!!!!!!!!!!!!!
            if (frameWithin == FrameWithin.Right)
            {
                velocity += velocity.x > 0 ? Vector3.right * 0.5f : Vector3.left * 0.25f;
            }
            else
            {
                velocity += velocity.z > 0 ? Vector3.forward * 0.5f : Vector3.back * 0.25f;
            }
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        Debug.Log(frameWithin.ToString() + ", " + m_Rigidbody.position.ToString() + ", " + velocity.ToString()); //!!!!!!!!!!!!!!

        m_Rigidbody.velocity = velocity;
    }
}
