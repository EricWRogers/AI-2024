using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public float maxAlignmentDistance = 3.0f;
    public float maxCohesionDistance = 2.0f;
    public float maxSeparationDistance = 0.5f;
    public float targetWeight = 0.35f;
    public float separationWeight = 1.0f;
    public float alignmentWeight = 0.4f;
    public float cohesionWeight = 0.15f;
    public float drag = 0.95f;
    public float speed = 1.0f;
    public float maxSpeed = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Boid[] boids = GetComponentsInChildren<Boid>();

        foreach(Boid boid in boids)
        {
            Vector2 pos = new Vector2(boid.transform.position.x, boid.transform.position.y);

            Vector2 seekDirection = Seek(pos, targetPos);
            Vector2 separationDirection = Separation(boid, pos);

            boid.acceleration = (seekDirection * targetWeight) +
                                (separationDirection * separationWeight);

            boid.velocity += boid.acceleration * speed * Time.deltaTime;

            // make the boid look the direction it is going
            boid.transform.right = new Vector3(boid.velocity.x, boid.velocity.y, 0.0f).normalized;

            // handle max speed
            boid.speed = boid.velocity.magnitude;

            if (boid.speed > maxSpeed)
            {
                boid.velocity = boid.velocity.normalized * maxSpeed;
            }

            boid.velocity *= drag;

            pos += boid.velocity;

            boid.transform.position = new Vector3(pos.x, pos.y, boid.transform.position.z);
        }
    }

    Vector2 Seek(Vector2 _agentPos, Vector2 _targetPos)
    {
        Vector2 seek = _targetPos - _agentPos;
        return seek.normalized;
    }

    Vector2 Separation(Boid _boid, Vector2 _agentPos)
    {
        Vector2 separation = Vector2.zero;
        int numberOfNeighbors = transform.childCount;

        Boid[] boids = GetComponentsInChildren<Boid>();

        foreach(Boid neighborBoid in boids)
        {
            if (_boid.gameObject != neighborBoid.gameObject)
            {
                Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
                float distance = Vector2.Distance(_agentPos, neighborPos);
                if (distance < maxSeparationDistance)
                {
                    // separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                    separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                }
            }
        }

        if (separation != Vector2.zero)
        {
            separation.Normalize();
        }

        return separation;
    }
}
