using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

#if (UNITY_EDITOR)
[CustomEditor(typeof(Battery))]
public class BatteryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Battery battery = (Battery)target;
        if (GUILayout.Button("Full Charge"))
        {
            battery.FullCharge();
        }
    }
}
#endif
public class Battery : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float powerLevel = 1.0f;
    public float lowPowerLevel = 0.2f;
    public bool lowPower = false;
    public bool charging = false;
    public UnityEvent enteredLowPowerMode;
    public UnityEvent fullyCharged;

    private float watts = 100.0f;
    private float maxWatts = 100.0f;

    public bool UseBattery(float _cost)
    {
        watts -= _cost;

        powerLevel = (watts <= 0.0f) ? 0.0f : watts/maxWatts;

        if (lowPower == false && powerLevel <= lowPowerLevel)
        {
            enteredLowPowerMode.Invoke();
        }

        lowPower = powerLevel <= lowPowerLevel;

        powerLevel = Mathf.Clamp(powerLevel, 0.0f, 1.0f);

        return powerLevel > 0.0f;
    }

    public void ChargeBattery(float _amount)
    {
        if (powerLevel >= 1.0f)
            return;
        
        float beforeCharge = watts;

        watts += _amount;

        watts = (watts > maxWatts) ? maxWatts : watts;

        powerLevel = (watts <= 0.0f) ? 0.0f : watts/maxWatts;

        if (powerLevel >= 1.0f)
            FullCharge();
    }

    public void FullCharge()
    {
        watts = maxWatts;
        powerLevel = 1.0f;
        lowPower = false;
        fullyCharged.Invoke();
    }
}
