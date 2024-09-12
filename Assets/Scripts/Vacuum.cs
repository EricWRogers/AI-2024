using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    public DirtManager dirtManager;
    public float speed = 30f;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 direction = Vector2.zero;

        direction.x = Input.GetAxis("Horizontal"); // -1 , 0, or 1
        direction.y = Input.GetAxis("Vertical");

        rigidbody2D.MovePosition(transform.position + (direction * speed * Time.fixedDeltaTime));

        List<Transform> dirtPile = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x/2.0f);

        dirtManager.RemoveDirt(dirtPile);
    }
}
