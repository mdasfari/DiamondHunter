using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public string CurrentState { get; private set; } // The key of the current state.
    public Dictionary<string, State> States { get; private set; } // Dictionary to hold all the states.

    // Constructor to initialize the Finite State Machine with an initial state.
    public FiniteStateMachine(State initialState)
    {
        States = new Dictionary<string, State>(); // Initialize the dictionary.

        CurrentState = initialState.StateKey; // Set the current state key.
        States.Add(CurrentState, initialState); // Add the initial state to the dictionary.
        initialState.Enter(); // Call the Enter method of the initial state.
    }

    // Method to update the logic of the current state.
    public bool LogicUpdate()
    {
        if (!States.ContainsKey(CurrentState)) // Check if the current state key exists in the dictionary.
            return false;

        States[CurrentState].LogicUpdate(); // Call the LogicUpdate method of the current state.
        return true;
    }

    // Method to update the physics of the current state.
    public bool PhysicsUpdate()
    {
        if (!States.ContainsKey(CurrentState)) // Check if the current state key exists in the dictionary.
            return false;

        States[CurrentState].PhysicsUpdate(); // Call the PhysicsUpdate method of the current state.
        return true;
    }

    // Method to change the current state to a new state.
    public bool ChangeState(string newStateKey)
    {
        if (!States.ContainsKey(newStateKey)) // Check if the new state key exists in the dictionary.
            return false;

        States[CurrentState].Exit(); // Call the Exit method of the current state.
        CurrentState = newStateKey; // Set the new state key as the current state.
        States[CurrentState].Enter(); // Call the Enter method of the new state.

        return true;
    }
}
