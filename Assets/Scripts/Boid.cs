using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 velocity;
    public Vector2 acceleration;
    public float speed = 0.0f;
    public Vector3 right;
    public Vector3 position;
    public List<Boid> boids;
    public List<Vector2> rocks;
    public float deltaTime;
}
