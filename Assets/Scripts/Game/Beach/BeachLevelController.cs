using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachLevelController : MonoBehaviour
{
    // Section for intro settings.
    [Header("Intro")]

    // Section for animation settings.
    [Header("Animation Settings")]
    public Transform shipObject; // Reference to the Transform component of the ship object.
    public Transform playerObject; // Reference to the Transform component of the player object.
    public float SailSpeed = 20f; // The speed at which the ship sails.

    // The final positions for the ship and player objects.
    public Vector2 shipEndPosition = new Vector2(20.07f, 0.14f);
    public Vector2 playerEndPosition = new Vector2(-9.7f, -3.237f);

    private FiniteStateMachine stateMachine; // Reference to the Finite State Machine for controlling game states.

    // Called before the first frame update; initializes the state machine.
    void Start()
    {
        IntroState introState = new IntroState(this); // Create an instance of the IntroState class.
        stateMachine = new FiniteStateMachine(introState); // Initialize the state machine with the intro state.
    }

    // Called once per frame; handles game logic updates.
    private void Update()
    {
        stateMachine.LogicUpdate(); // Update the logic of the current state in the state machine.
    }

    // Called at a fixed interval; handles physics-related updates.
    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate(); // Update the physics of the current state in the state machine.
    }
}
