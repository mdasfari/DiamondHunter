using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; } // Property to hold the current state of the player.

    // Method to initialize the state machine with a starting state.
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState; // Assign the starting state to the current state.
        CurrentState.Enter(); // Call the Enter method of the current state to activate it.
    }

    // Method to change the current state to a new state.
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit(); // Call the Exit method of the current state to deactivate it.
        CurrentState = newState; // Assign the new state to the current state.
        CurrentState.Enter(); // Call the Enter method of the new state to activate it.
    }
}
