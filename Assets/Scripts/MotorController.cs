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
    private Rigidbody2D rb2d;
    private Vector3 lastPos;
    private bool goForward = false;
    private float targetRotation = 0;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        lastPos = transform.position;
        targetRotation = transform.eulerAngles.z;
    }

    void FixedUpdate()
    {
        float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetRotation);
        turning = Mathf.Abs(angleDifference) > 0.01f;

        if (turning)
        {
            float newAngle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.z,
                targetRotation,
                turningSpeed * Time.deltaTime
            );

            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                newAngle
            );

            if (Mathf.Approximately(newAngle, targetRotation))
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    targetRotation
                );
                
                turning = false;
                finishedTurning.Invoke();
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
            
            rb2d.MovePosition(transform.position + (transform.right * speed * Time.fixedDeltaTime));

            goForward = false;
        }
    }

    public void Forward()
    {
        goForward = true;
    }

    public void Turn(float _dergree)
    {
        if (turning)
            return;
        
        goForward = false;
        targetRotation += _dergree;
    }
}
