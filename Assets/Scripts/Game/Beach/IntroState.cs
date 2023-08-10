using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : State
{
    private BeachLevelController levelController;

    private Transform currentShipTransform;
    private Transform currentPlayerTransform;

    public IntroState(BeachLevelController levelController) : base("IntroState")
    {
        this.levelController = levelController;

        currentShipTransform = levelController.shipObject;
        currentPlayerTransform = levelController.playerObject;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (levelController.shipObject.transform.position.x < levelController.shipEndPosition.x)
        {
            levelController.shipObject.transform.position = Vector3.right * (levelController.shipObject.transform.position.x + levelController.SailSpeed);
            levelController.playerObject.transform.position = Vector3.right * (levelController.playerObject.transform.position * Vector2.right * levelController.SailSpeed);

            Debug.Log(Vector3.right * (levelController.shipObject.transform.position.x + levelController.SailSpeed));
        }
    }

    public override void UpdateStatus()
    {
        base.UpdateStatus();
    }
}
