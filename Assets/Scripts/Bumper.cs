using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bumper : MonoBehaviour
{
    public Transform bumperSprite;
    public bool hittingObject = false;
    public UnityEvent hit;
    public LayerMask mask;

    void FixedUpdate()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(bumperSprite.position, 0.5f, transform.right, 0.0f, ~mask, -Mathf.Infinity, Mathf.Infinity);

        if (hittingObject == false && hits.Length > 0)
            hit.Invoke();
        
        hittingObject = (hits.Length > 0);
    }
}
