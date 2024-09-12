using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    public DirtManager dirtManager;
    public float speed = 30f;

    void FixedUpdate()
    {
        Vector3 direction = Vector2.zero;

        direction.x = Input.GetAxis("Horizontal"); // -1 , 0, or 1
        direction.y = Input.GetAxis("Vertical");

        transform.position += direction * speed * Time.fixedDeltaTime;

        List<Transform> dirtPile = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x/2.0f);

        dirtManager.RemoveDirt(dirtPile);
    }
}
