using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotorController : MonoBehaviour
{
    public float speed = 30f;
    public float turningSpeed = 5.0f;
    public float distanceTraveled;
    public bool turning = false;
    public UnityEvent finishedTurning;
    private Rigidbody2D rigidbody2D;
    private Vector3 lastPos;
    private bool goForward = false;
    private float targetRotation = 0;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        lastPos = transform.position;
        targetRotation = transform.eulerAngles.z;
    }

    void FixedUpdate()
    {
        turning = (targetRotation != transform.eulerAngles.z);

        if (turning)
        {
            if (targetRotation < transform.eulerAngles.z)
            {
                transform.Rotate(0.0f, 0.0f, -turningSpeed * Time.fixedDeltaTime);

                if (targetRotation > transform.eulerAngles.z)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y,
                        targetRotation
                    );
                    finishedTurning.Invoke();
                }
            }
            else
            {
                transform.Rotate(0.0f, 0.0f, turningSpeed * Time.fixedDeltaTime);

                if (targetRotation < transform.eulerAngles.z)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y,
                        targetRotation
                    );
                    finishedTurning.Invoke();
                }
            }
        }

        if (goForward && turning)
        {
            GameObject.FindWithTag("HUD").GetComponent<HUD>().MotorBroke();
            return;
        }

        if (goForward)
        {
            if (lastPos != transform.position)
            distanceTraveled += Vector3.Distance(lastPos, transform.position);

            lastPos = transform.position;
            
            rigidbody2D.MovePosition(transform.position + (transform.right * speed * Time.fixedDeltaTime));

            goForward = false;
        }
    }

    public void Forward()
    {
        goForward = true;
    }

    public void Turn(float _dergree)
    {
        targetRotation += _dergree;
    }
}
