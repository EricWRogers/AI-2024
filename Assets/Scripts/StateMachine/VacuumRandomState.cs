using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPupSystems.StateMachine;

[System.Serializable]
public class VacuumRandomState : SimpleState
{
    private VacuumStateMachine vacuumStateMachine;
    private DirtManager dirtManager;
    private MotorController motorController;
    public override void OnStart()
    {
        vacuumStateMachine = (VacuumStateMachine)stateMachine;
        dirtManager = vacuumStateMachine.dirtManager;
        motorController = stateMachine.GetComponent<MotorController>();
        stateMachine.GetComponent<Bumper>().hit.AddListener(Bump);
    }

    public override void UpdateState(float _dt)
    {
        if (motorController.turning == false)
            motorController.Forward();

        List<GameObject> dirtPile = dirtManager.FindDirtInCircle(stateMachine.transform.position, stateMachine.transform.localScale.x/2.0f);

        dirtManager.RemoveDirt(dirtPile);
    }

    public override void OnExit()
    {
        stateMachine.GetComponent<Bumper>().hit.RemoveListener(Bump);
    }

    public void Bump()
    {
        motorController.Turn(Random.Range(85.0f, 95.0f));
    }
}
