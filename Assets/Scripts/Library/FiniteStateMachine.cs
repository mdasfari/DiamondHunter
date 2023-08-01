using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public string CurrentState { get; private set; }
    public Dictionary<string, State> States { get; private set; }

    public FiniteStateMachine(State initialState)
    {
        States = new Dictionary<string, State>();

        CurrentState = initialState.StateKey;
        States.Add(CurrentState, initialState);
        initialState.Enter();
    }

    public bool LogicUpdate()
    {
        if (!States.ContainsKey(CurrentState))
            return false;

        States[CurrentState].LogicUpdate();
        return true;
    }

    public bool PhysicsUpdate()
    {
        if (!States.ContainsKey(CurrentState))
            return false;

        States[CurrentState].PhysicsUpdate();

        return true;
    }

    public bool ChangeState(string newStateKey)
    {
        if (!States.ContainsKey(newStateKey))
            return false;

        States[CurrentState].Exit();
        CurrentState = newStateKey;
        States[CurrentState].Enter();

        return true;
    }
}
