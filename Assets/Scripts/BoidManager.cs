using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;

// 24fps ~ 200 boids
// 29fps ~ 200 boids
// 80fps ~ 200 boids oop quadtree
// 30fps ~ 400 boids

public class BoidManager : MonoBehaviour
{
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

    private Boid[] boids;
    private List<Boid> neighborBoids = new List<Boid>();
    QuadTree rockTree = new QuadTree(Vector2.zero, 1000.0f, 1000.0f);

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("ROCK");

        foreach (GameObject go in gos)
        {
            rockTree.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        boids = GetComponentsInChildren<Boid>();

        QuadTree quadTree = new QuadTree(Vector2.zero, 1000.0f, 1000.0f);

        foreach (Boid boid in boids)
        {
            quadTree.Add(boid.gameObject);
        }

        Vector2 pos;

        Vector2 fleeDirection;
        Vector2 seekDirection;
        Vector2 separationDirection;
        Vector2 alignmentDirection;
        Vector2 cohesionDirection;

        foreach(Boid boid in boids)
        {
            pos = new Vector2(boid.transform.position.x, boid.transform.position.y);

            neighborBoids = quadTree.FindComponent<Boid>(pos, maxAlignmentDistance);

            fleeDirection = Flee(pos);
            seekDirection = Seek(pos, targetPos);

            int alignmentNON = 0;
            int cohesionNON = 0;

            separationDirection = Vector2.zero;
            alignmentDirection = Vector2.zero;
            cohesionDirection = Vector2.zero;

            foreach (Boid neighborBoid in neighborBoids)
            {
                if (boid.gameObject != neighborBoid.gameObject)
                {
                    Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
                    float distance = Vector2.Distance(pos, neighborPos);

                    if (distance < maxAlignmentDistance)
                    {
                        alignmentNON++;
                        alignmentDirection += neighborBoid.velocity;

                        if (distance < maxCohesionDistance)
                        {
                            cohesionDirection += neighborPos;
                            cohesionNON++;

                            if (distance < maxSeparationDistance)
                            {
                                // this give a linear falloff to the separation effects
                                separationDirection += (pos - neighborPos).normalized * (maxSeparationDistance - distance);
                            }
                        }
                    }
                }
            }

            if (separationDirection != Vector2.zero)
            {
                separationDirection.Normalize();
            }

            if (alignmentNON > 0)
            {
                alignmentDirection = (alignmentDirection / (float)alignmentNON).normalized;
            }

            if (cohesionNON > 0)
            {
                // gets the average neighbor pos
                cohesionDirection /= (float)cohesionNON;

                cohesionDirection -= pos;

                if (cohesionDirection != Vector2.zero)
                {
                    cohesionDirection = cohesionDirection.normalized;
                }
            }


            //separationDirection = Separation(boids[i], pos);
            //alignmentDirection = Alignment(boids[i], pos);
            //cohesionDirection = Cohesion (boids[i], pos);

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

        List<GameObject> rocks = rockTree.Find(_agentPos, maxBombDistance);

        foreach (GameObject bomb in rocks)
        {
            Vector2 bombPos = new Vector2(bomb.transform.position.x, bomb.transform.position.y);
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

        foreach (Boid neighborBoid in neighborBoids)
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

        foreach (Boid neighborBoid in neighborBoids)
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

        foreach (Boid neighborBoid in neighborBoids)
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
