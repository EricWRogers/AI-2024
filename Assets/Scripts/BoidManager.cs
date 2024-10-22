using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.TextCore;

public class BoidManager : MonoBehaviour
{
    public List<Transform> bombs;
    public float maxAlignmentDistance = 3.0f;
    public float maxCohesionDistance = 2.0f;
    public float maxSeparationDistance = 0.5f;
    public float maxBombDistance = 1.0f;
    public float fleeWeight = 1.0f;
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

            Vector2 fleeDirection = Flee(pos);
            Vector2 seekDirection = Seek(pos, targetPos);
            Vector2 separationDirection = Separation(boid, pos);
            Vector2 alignmentDirection = Alignment(boid, pos);
            Vector2 cohesionDirection = Cohesion (boid, pos);

            boid.acceleration = (seekDirection * targetWeight) +
                                (separationDirection * separationWeight) + 
                                (alignmentDirection * alignmentWeight) +
                                (cohesionDirection * cohesionWeight) + 
                                (fleeDirection * fleeWeight);

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

    Vector2 Flee(Vector2 _agentPos)
    {
        Vector2 flee = Vector2.zero;

        foreach(Transform bomb in bombs)
        {
            Vector2 bombPos = new Vector2(bomb.position.x, bomb.position.y);
            float distance = Vector2.Distance(_agentPos, bombPos);
            if (distance < maxBombDistance)
            {
                Vector2 direction = _agentPos - bombPos;
                flee += direction;
            }
        }

        if (flee != Vector2.zero)
            flee = flee.normalized;
        
        return flee;
    }

    Vector2 Seek(Vector2 _agentPos, Vector2 _targetPos)
    {
        Vector2 seek = _targetPos - _agentPos;
        return seek.normalized;
    }

    Vector2 Separation(Boid _boid, Vector2 _agentPos)
    {
        Vector2 separation = Vector2.zero;

        Boid[] boids = GetComponentsInChildren<Boid>();

        foreach(Boid neighborBoid in boids)
        {
            if (_boid.gameObject != neighborBoid.gameObject)
            {
                Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
                float distance = Vector2.Distance(_agentPos, neighborPos);
                if (distance < maxSeparationDistance)
                {
                    // this give a linear falloff to the separation effects
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

    Vector2 Alignment(Boid _boid, Vector2 _agentPos)
    {
        Vector2 alignment = Vector2.zero;
        int numberOfNeighbors = 0;

        Boid[] boids = GetComponentsInChildren<Boid>();

        foreach(Boid neighborBoid in boids)
        {
            if (_boid.gameObject != neighborBoid.gameObject)
            {
                Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
                float distance = Vector2.Distance(_agentPos, neighborPos);
                if (distance < maxAlignmentDistance)
                {
                    numberOfNeighbors++;
                    alignment += neighborBoid.velocity;
                }
            }
        }

        if (numberOfNeighbors > 0)
        {
            return (alignment / (float)numberOfNeighbors).normalized;
        }

        return Vector2.zero;
    }

    Vector2 Cohesion(Boid _boid, Vector2 _agentPos)
    {
        Vector2 cohesion = Vector2.zero;
        int numberOfNeighbors = 0;

        Boid[] boids = GetComponentsInChildren<Boid>();

        foreach (Boid neighborBoid in boids)
        {
            Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
            float distance = Vector2.Distance(_agentPos, neighborPos);

            if (distance < maxCohesionDistance)
            {
                cohesion += neighborPos;
                numberOfNeighbors++;
            }
        }

        if (numberOfNeighbors > 0)
        {
            // gets the average neighbor pos
            cohesion /= (float)numberOfNeighbors;

            cohesion -= _agentPos;

            if (cohesion != Vector2.zero)
            {
                return cohesion.normalized;
            }
        }

        return Vector2.zero;
    }
}
