using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Rigidbody rb;
    public Joystick joystick;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    //  public float forwardForce;
    // public float sidewayForce;
    float moveForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
       /* forwardForce = sidewayForce =*/ moveForce= transform.localScale.x*8;
    }

    void JoystickMovement()
    {
        horizontalMove = joystick.Horizontal * moveForce;
        verticalMove = joystick.Vertical * moveForce;
         rb.velocity = new Vector3 (horizontalMove, rb.velocity.y, verticalMove);
      

        //if (verticalMove > 0f)
        //{
        //    rb.AddForce(0, 0, 100 * Time.deltaTime);
        //    rb.AddTorque(forwardForce * Time.deltaTime, 0, 0);
        //}
        //if (verticalMove < 0f)
        //{
        //    rb.AddForce(0, 0, -100 * Time.deltaTime);
        //    rb.AddTorque(-forwardForce * Time.deltaTime, 0, 0);
        //}
        //if (horizontalMove > 0f)
        //{
        //    rb.AddForce(100 * Time.deltaTime, 0, 0);
        //    rb.AddTorque(0, 0, -sidewayForce * Time.deltaTime);
        //}
        //if (horizontalMove < 0f)
        //{
        //    rb.AddForce(-100 * Time.deltaTime, 0, 0);
        //    rb.AddTorque(0, 0, sidewayForce * Time.deltaTime);
        //}

    }
    private void FixedUpdate()
    {
        JoystickMovement();
    }
    private void OnCollisionStay(Collision collision)
    {
        //JoystickMovement();
    }
}