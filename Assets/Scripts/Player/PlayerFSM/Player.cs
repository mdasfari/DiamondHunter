﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    #region Variables

    private CameraFollowObject cameraFollowObject; // Reference to the camera follow object for smooth camera movement.
    public Animator Anim { get; private set; } // Reference to the Animator component to control animations.
    public PlayerInputHandler InputHandler { get; private set; } // Reference to the input handler for player input.
    public Rigidbody2D rb { get; private set; } // Reference to the Rigidbody2D component for physics.

    public Vector2 CurrentVelocity { get; private set; } // Stores the current velocity of the player.
    public int FaceDirection { get; private set; } // Stores the direction the player is facing (1 for right, -1 for left).

    [Header("Others")]
    [SerializeField]
    private PlayerData playerData; // Reference to the player data scriptable object containing various settings.

    private Vector2 workspace; // Temporary variable used for various calculations.
    private float fallSpeedYDampingChangeThrehold; // Threshold for changing the Y damping of the fall speed.
    private AudioSource audioSource; // Reference to the AudioSource component to play sounds.

    #endregion

    #region FSM State

    // Finite State Machine (FSM) states for the player.
    public PlayerStateMachine StateMachine { get; private set; } // The state machine managing player states.
    public PlayerIdleState IdleState { get; private set; } // State when the player is idle.
    public PlayerMoveState MoveState { get; private set; } // State when the player is moving.
    public PlayerJumpState JumpState { get; private set; } // State when the player is jumping.
    public PlayerAirState AirState { get; private set; } // State when the player is in the air.
    public PlayerLandState LandState { get; private set; } // State when the player lands.
    public PlayerWallSlideState WallSlideState { get; private set; } // State when the player is sliding on a wall.
    public PlayerWallGrapState WallGrapState { get; private set; } // State when the player is grabbing a wall.
    public PlayerWallClimbState WallClimbState { get; private set; } // State when the player is climbing a wall.
    public PlayerWallJumpState WallJumpState { get; private set; } // State when the player is jumping off a wall.

    #endregion

    #region Transform Variable

    [SerializeField]
    private Transform GroundedCheck; // Transform for checking if the player is grounded.
    [SerializeField]
    private Transform WalledCheck; // Transform for checking if the player is near a wall.

    #endregion

    #region FoundationFunction

    private void Awake()
    {
        // Initialize FSM states.
        StateMachine = new PlayerStateMachine(); // Create the state machine.
        // Initialize the various states with references to this player object, the state machine, and player data.
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "air");
        AirState = new PlayerAirState(this, StateMachine, playerData, "air");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrapState = new PlayerWallGrapState(this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "air");
    }

    private void Start()
    {
        // Get components and initialize variables.
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component.
        Anim = GetComponent<Animator>(); // Get the Animator component.
        InputHandler = GetComponent<PlayerInputHandler>(); // Get the PlayerInputHandler component.
        cameraFollowObject = GameObject.FindGameObjectWithTag("CinaCamera").GetComponent<CameraFollowObject>(); // Get the CameraFollowObject component.
        StateMachine.Initialize(IdleState); // Initialize the state machine with the idle state.
        FaceDirection = 1; // Set the initial face direction to the right.
        fallSpeedYDampingChangeThrehold = CameraManager.instance.fallSpeedYDampingChangeThreshold; // Get the fall speed Y damping change threshold from the camera manager.
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
    }

    private void Update()
    {
        CurrentVelocity = rb.velocity; // Update the current velocity from the Rigidbody2D component.
        StateMachine.CurrentState.LogicUpdate(); // Call the LogicUpdate method of the current state.

        // Check if the player is falling past a certain speed threshold.
        if (rb.velocity.y < fallSpeedYDampingChangeThrehold
            && !CameraManager.instance.IsLerpingYDamping
            && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true); // Lerp the Y damping if falling fast.
        }

        // Check if the player is standing still or moving up.
        if (rb.velocity.y >= 0f
            && !CameraManager.instance.IsLerpingYDamping
            && CameraManager.instance.LerpedFromPlayerFalling)
        {
            // Reset so it can be called again.
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false); // Reset the Y damping if not falling.
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate(); // Call the PhysicsUpdate method of the current state.
    }

    #endregion 

    public void PlaySound(PlayerAudioFiles audioFile, bool playLoop = false)
    {
        // Play sound based on the selected audio file.
        AudioClip selectedAudio = null; // Variable to hold the selected audio clip.

        // Switch statement to select the appropriate audio clip based on the provided enum value.
        switch (audioFile)
        {
            case PlayerAudioFiles.Walk:
                selectedAudio = playerData.Walk;
                break;
            case PlayerAudioFiles.Jump:
                selectedAudio = playerData.Jump;
                break;
            case PlayerAudioFiles.Sword:
                selectedAudio = playerData.Sword;
                break;
            case PlayerAudioFiles.Throw:
                selectedAudio = playerData.Throw;
                break;
            case PlayerAudioFiles.Respawn:
                selectedAudio = playerData.Respawn;
                break;
            case PlayerAudioFiles.Damage:
                selectedAudio = playerData.Damage;
                break;
            case PlayerAudioFiles.LostLife:
                selectedAudio = playerData.LostLife;
                break;
        }

        if (selectedAudio != null)
        {
            audioSource.loop = playLoop;
            audioSource.clip = selectedAudio;
            audioSource.Play();
        }
    }

    internal void StopPlaySound()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
    public void SetVolcityX(float velocity)
    {
        // Set the X component of the velocity.
        workspace.Set(velocity, CurrentVelocity.y); // Set the workspace vector with the new X velocity and current Y velocity.
        rb.velocity = workspace; // Assign the workspace vector to the Rigidbody2D velocity.
        CurrentVelocity = workspace; // Update the current velocity.
    }

    public void SetVolcityY(float velocity)
    {
        // Set the Y component of the velocity.
        workspace.Set(CurrentVelocity.x, velocity); // Set the workspace vector with the current X velocity and new Y velocity.
        rb.velocity = workspace; // Assign the workspace vector to the Rigidbody2D velocity.
        CurrentVelocity = workspace; // Update the current velocity.
    }

    public void SetVelocity(float velocity, Vector2 Angle, int direction)
    {
        // Set the velocity based on the provided angle and direction.
        Angle.Normalize(); // Normalize the angle vector.
        workspace.Set(Angle.x * velocity * direction, Angle.y * velocity); // Calculate the new velocity based on the angle and direction.
        rb.velocity = workspace; // Assign the workspace vector to the Rigidbody2D velocity.
        CurrentVelocity = workspace; // Update the current velocity.
    }

    public bool CheckIfGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(GroundedCheck.position, playerData.groundCheckRadius, playerData.GroundFloor);
        return grounded;
    }

    public bool CheckIfWalled()
    {
        bool nearWall = Physics2D.Raycast(WalledCheck.position, Vector2.right * FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
        return nearWall;
    }

    public bool CheckIfWalledBack()
    {
        bool nearBackWall = Physics2D.Raycast(WalledCheck.position, Vector2.right * -FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
        return nearBackWall;
    }

    private void AnimationTrigger()
    {
        // Call the AnimationTrigger method of the current state.
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationTriggerFinished()
    {
        // Call the AnimationTriggerFinished method of the current state.
        StateMachine.CurrentState.AnimationTriggerFinished();
    }

    public void CheckFlipFace(int xInput)
    {
        // Check if the player needs to flip face direction based on the X input.
        if (xInput != 0 && xInput != FaceDirection)
            FlipFace(); // Call the FlipFace method if the X input is not zero and not equal to the current face direction.
    }

    private void FlipFace()
    {
        // Flip the face direction of the player.
        FaceDirection *= -1; // Multiply the face direction by -1 to flip it.
        transform.Rotate(0f, 180f, 0f); // Rotate the player's transform by 180 degrees around the Y axis.
        cameraFollowObject.CallTurn(); // Call the camera follow object's turn method.
    }
}
