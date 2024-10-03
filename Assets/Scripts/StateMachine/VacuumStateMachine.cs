using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPupSystems.StateMachine;

public class VacuumStateMachine : SimpleStateMachine
{
    public DirtManager dirtManager;

    public VacuumRandomState vacuumRandomState;

    void Awake()
    {
        // add all states first
        states.Add(vacuumRandomState);
    }

    void Start()
    {
        // start the first state
        ChangeState(nameof(vacuumRandomState));
    }

    void Update()
    {

    }
}
