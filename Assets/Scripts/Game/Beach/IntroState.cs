using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : State
{
    private BeachLevelController levelController; // Reference to the BeachLevelController.

    private Transform currentShipTransform; // Current transform of the ship object.
    private Transform currentPlayerTransform; // Current transform of the player object.

    // Constructor to initialize the IntroState with the BeachLevelController.
    public IntroState(BeachLevelController levelController) : base("IntroState")
    {
        this.levelController = levelController;

        currentShipTransform = levelController.shipObject; // Initialize the ship's transform.
        currentPlayerTransform = levelController.playerObject; // Initialize the player's transform.
    }

    // Called when entering the state.
    public override void Enter()
    {
        base.Enter();
    }

    // Called when exiting the state.
    public override void Exit()
    {
        base.Exit();
    }

    // Called once per frame to update the game logic.
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Check if the ship's position is less than the end position.
        if (levelController.shipObject.transform.position.x < levelController.shipEndPosition.x)
        {
            // Move the ship and player objects to the right based on the sailing speed.
            levelController.shipObject.transform.position = Vector3.right * (levelController.shipObject.transform.position.x + levelController.SailSpeed);
            levelController.playerObject.transform.position = Vector3.right * (levelController.playerObject.transform.position * Vector2.right * levelController.SailSpeed);

            // Log the new position of the ship for debugging purposes.
            Debug.Log(Vector3.right * (levelController.shipObject.transform.position.x + levelController.SailSpeed));
        }
    }

    // Called to update the status of the state (currently empty).
    public override void UpdateStatus()
    {
        base.UpdateStatus();
    }
}
