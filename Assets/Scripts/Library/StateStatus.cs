using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumeration representing the possible statuses for a state within the FSM.
public enum StateStatus 
{
    Idle,           // The state is idle and not active.
    Enter,          // The state is being entered.
    LogicUpdate,    // The state is performing logic updates.
    PhysicsUpdate,  // The state is performing physics updates.
    Exit,           // The state is being exited.
}

// Enumeration representing the possible statuses for an animation within a state.
public enum StateAnimationStatus
{
    Idle,   // The animation is idle and not active.
    Start,  // The animation is starting.
    Finish  // The animation has finished.
}