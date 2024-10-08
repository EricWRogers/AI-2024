using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    public Transform playerTransform;

    public float rateOfChargeInWatts = 100.0f;
    private bool wasCharging = false;
    
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, playerTransform.position));
        if (Vector3.Distance(transform.position, playerTransform.position) < transform.localScale.x/2.0f)
        {
            wasCharging = true;
            Battery battery = playerTransform.GetComponent<Battery>();
            battery.charging = true;
            battery.ChargeBattery(rateOfChargeInWatts * Time.deltaTime);
        }
        else
        {
            if (wasCharging)
            {
                wasCharging = false;
                Battery battery = playerTransform.GetComponent<Battery>();
                battery.charging = false;
            }
        }
    }
}
