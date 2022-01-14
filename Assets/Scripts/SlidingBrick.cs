using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingBrick : Brick
{
    Rigidbody m_Rigidbody;
    private float speed = 1.0f;

    //This sets the desired velocity, and when getting remembers what the last set desired velocity was 
    //(even if the actual velocity changed because of collision, etc.)
    private Vector3 m_DesiredVelocity;
    private Vector3 DesiredVelocity
    {
        get
        {
            return m_DesiredVelocity;
        }
        set
        {
            m_DesiredVelocity = value;
            m_Rigidbody.velocity = m_DesiredVelocity;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        //Initialize Constraints and speed
        Vector3 initialDirection = gameObject.transform.TransformVector(Vector3.right).normalized;
        if (initialDirection == Vector3.right)
        {
            m_Rigidbody.constraints = m_Rigidbody.constraints | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            m_Rigidbody.constraints = m_Rigidbody.constraints | RigidbodyConstraints.FreezePositionX;
        }

        DesiredVelocity = initialDirection * speed;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);

        //If collides with something other than the ball, reverse velocity.
        if (other.gameObject.CompareTag("Ball") == false)
        {
            DesiredVelocity = -m_DesiredVelocity;
        }
    }
}
