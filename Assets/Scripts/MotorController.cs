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
    public float onBumpReverseDistance = 0.2f;
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
        if (turning)
        {
            Quaternion targetRotationQuaternion = Quaternion.Euler(0, 0, targetRotation);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotationQuaternion,
                turningSpeed * Time.fixedDeltaTime
            );

            if (Mathf.Abs(Quaternion.Angle(transform.rotation, targetRotationQuaternion)) < 0.1f)
            {
                transform.rotation = targetRotationQuaternion;
                turning = false;
                finishedTurning.Invoke();
                return;
            }
            return;
        }

        if (goForward && !turning)
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

    public void Turn(float _degree)
    {
        if (turning)
            return;
        
        turning = true;
        goForward = false;
        
        targetRotation = (targetRotation + _degree) % 360;
        if (targetRotation < 0)
            targetRotation += 360;
    }

    public void OnBump()
    {
        transform.position = transform.position + (-transform.right * onBumpReverseDistance);
    }
}
