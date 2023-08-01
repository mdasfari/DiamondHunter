using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public string StateKey { get; protected set; }

    public StateStatus CurrentStatus { get; private set; }
    public StateAnimationStatus CurrentAnimationStatus { get; private set; }

    public float StartTime { get; private set; }

    public State(string InitialStateKey)
    {
        CurrentStatus = StateStatus.Idle;
        CurrentAnimationStatus = StateAnimationStatus.Idle;
        StateKey = InitialStateKey;
    }

    public virtual void Enter()
    {
        CurrentStatus = StateStatus.Enter;
        StartTime = Time.time;

        UpdateStatus();

        DisplayDebugMessage();
    }

    public virtual void Exit()
    {
        CurrentStatus = StateStatus.Exit;
    }

    public virtual void LogicUpdate()
    {
        CurrentStatus = StateStatus.LogicUpdate;

        UpdateStatus();

        DisplayDebugMessage();
    }

    public virtual void PhysicsUpdate()
    {
        CurrentStatus = StateStatus.PhysicsUpdate;

        UpdateStatus();

        DisplayDebugMessage();
    }

    public virtual void AnimationEnter()
    {
        CurrentAnimationStatus = StateAnimationStatus.Start;

        DisplayDebugMessage();
    }

    public virtual void AnimationExit()
    {
        CurrentAnimationStatus = StateAnimationStatus.Finish;

        DisplayDebugMessage();
    }

    public virtual void UpdateStatus()
    {

    }

    private void DisplayDebugMessage()
    {
        string debugMessage = string.Format("State: {0} - Status: {1}, Start Time: {2}, Current Time: {3}", StateKey, CurrentStatus.ToString(), StartTime, Time.time);
        Debug.Log(debugMessage);
    }
}
