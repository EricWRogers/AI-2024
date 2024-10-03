using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCPU : MonoBehaviour
{
    public DirtManager dirtManager;
    private MotorController motorController;
    // Start is called before the first frame update
    void Start()
    {
        motorController = GetComponent<MotorController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (motorController.turning == false)
            motorController.Forward();

        List<GameObject> dirtPile = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x/2.0f);

        dirtManager.RemoveDirt(dirtPile);
    }

    public void Bump()
    {
        motorController.Turn(Random.Range(85.0f, 95.0f));
    }
}
