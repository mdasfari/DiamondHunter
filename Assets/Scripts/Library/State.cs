using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public string StateKey { get; protected set; } // Unique identifier for the state.

    public StateStatus CurrentStatus { get; private set; } // Current status of the state.
    public StateAnimationStatus CurrentAnimationStatus { get; private set; } // Current animation status.

    public float StartTime { get; private set; } // Start time of the state.

    // Constructor to initialize the state with a key.
    public State(string InitialStateKey)
    {
        CurrentStatus = StateStatus.Idle;
        CurrentAnimationStatus = StateAnimationStatus.Idle;
        StateKey = InitialStateKey;
    }

    // Called when entering the state.
    public virtual void Enter()
    {
        CurrentStatus = StateStatus.Enter;
        StartTime = Time.time;
        UpdateStatus();
        DisplayDebugMessage();
    }

    // Called when exiting the state.
    public virtual void Exit()
    {
        CurrentStatus = StateStatus.Exit;
    }

    // Called for logic updates within the state.
    public virtual void LogicUpdate()
    {
        CurrentStatus = StateStatus.LogicUpdate;
        UpdateStatus();
        DisplayDebugMessage();
    }

    // Called for physics updates within the state.
    public virtual void PhysicsUpdate()
    {
        CurrentStatus = StateStatus.PhysicsUpdate;
        UpdateStatus();
        DisplayDebugMessage();
    }

    // Called when entering an animation within the state.
    public virtual void AnimationEnter()
    {
        CurrentAnimationStatus = StateAnimationStatus.Start;
        DisplayDebugMessage();
    }

    // Called when exiting an animation within the state.
    public virtual void AnimationExit()
    {
        CurrentAnimationStatus = StateAnimationStatus.Finish;
        DisplayDebugMessage();
    }

    // Method to update the status of the state. Can be overridden by derived classes.
    public virtual void UpdateStatus()
    {

    }

    // Private method to display debug messages related to the state.
    private void DisplayDebugMessage()
    {
        string debugMessage = string.Format("State: {0} - Status: {1}, Start Time: {2}, Current Time: {3}", StateKey, CurrentStatus.ToString(), StartTime, Time.time);
        Debug.Log(debugMessage);
    }
}
