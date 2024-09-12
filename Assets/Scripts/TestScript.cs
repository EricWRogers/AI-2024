using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float countDown = 10.0f;
    private bool hidenVar = true;
    void Start()
    {
        Debug.Log("Start");

        bool i = true;

        if (i == hidenVar)
        {
            Debug.Log("HidenVar");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
    }
}
