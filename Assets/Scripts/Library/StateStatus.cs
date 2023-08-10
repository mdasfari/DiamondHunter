using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateStatus 
{
    Idle,           
    Enter,          
    LogicUpdate,    
    PhysicsUpdate,  
    Exit,           
}


public enum StateAnimationStatus
{
    Idle,   
    Start,  
    Finish  
}