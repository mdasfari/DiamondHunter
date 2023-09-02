using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    #region Variables

    private CameraFollowObject cameraFollowObject; 
    public Animator Anim { get; private set; }

    [SerializeField]
    public Animator WeaponAnim; // { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; } 
    public Rigidbody2D rb { get; private set; } 

    public Vector2 CurrentVelocity { get; private set; } 
    public int FaceDirection { get; private set; } 

    [Header("Others")]
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private PlayerData playerData; 

    private Vector2 workspace; 
    private float fallSpeedYDampingChangeThrehold; 
    private AudioSource audioSource; 

    public RamplingTypes ramplingType { get; private set; }

    #endregion

    public UnityEvent OnWeaponDrawTriggered;
    public void TriggerAttack()
    {
        OnWeaponDrawTriggered?.Invoke();
    }

    #region FSM State

    // Finite State Machine (FSM) states for the player.
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrapState WallGrapState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    public PlayerRamblingRopeState RamblingRopeState { get; private set; }
    public PlayerRamblingLadderState RamblingLadderState { get; private set; }

    public PlayerWeaponState WeaponState { get; private set; }

    #endregion

    #region Transform Variable

    [SerializeField]
    private Transform GroundedCheck;
    [SerializeField]
    private Transform WalledCheck;
    [SerializeField]
    private Transform RamblingCheck;
    [SerializeField]
    private Transform weaponBoundry;

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

        RamblingRopeState = new PlayerRamblingRopeState(this, StateMachine, playerData, "rope");
        RamblingLadderState = new PlayerRamblingLadderState(this, StateMachine, playerData, "ladder");

        WeaponState = new PlayerWeaponState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        // Get components and initialize variables.
        rb = GetComponent<Rigidbody2D>(); 
        Anim = GetComponent<Animator>(); 
        
        InputHandler = GetComponent<PlayerInputHandler>();

        cameraFollowObject = GameObject.FindGameObjectWithTag("CinaCamera").GetComponent<CameraFollowObject>();

        fallSpeedYDampingChangeThrehold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
        StateMachine.Initialize(IdleState); 
        FaceDirection = 1; 
        
        audioSource = GetComponent<AudioSource>(); 

        ramplingType = RamplingTypes.None;
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
            CameraManager.instance.LerpYDamping(true); 
        }

        // Check if the player is standing still or moving up.
        if (rb.velocity.y >= 0f
            && !CameraManager.instance.IsLerpingYDamping
            && CameraManager.instance.LerpedFromPlayerFalling)
        {
            // Reset so it can be called again.
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false); 
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate(); 
    }

    #endregion 

    public void PlaySound(PlayerAudioFiles audioFile, bool playLoop = false)
    {
        // Play sound based on the selected audio file.
        AudioClip selectedAudio = null; 

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
        workspace.Set(velocity, CurrentVelocity.y); 
        rb.velocity = workspace; 
        CurrentVelocity = workspace; 
    }

    public void SetVolcityY(float velocity)
    {
        // Set the Y component of the velocity.
        workspace.Set(CurrentVelocity.x, velocity); 
        rb.velocity = workspace; 
        CurrentVelocity = workspace; 
    }

    public void SetVelocity(float velocity, Vector2 Angle, int direction)
    {
        // Set the velocity based on the provided angle and direction.
        Angle.Normalize(); 
        workspace.Set(Angle.x * velocity * direction, Angle.y * velocity); 
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    private void OnTriggerEnter2D(Collider2D collsion)
    {
        ramplingType = RamplingTypes.None;
        if (Enum.TryParse(collsion.tag, out RamplingTypes result))
            ramplingType = result;
    }

    private void OnTriggerExit2D(Collider2D collsion)
    {
        ramplingType = RamplingTypes.None;
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

    public bool CheckIfRambling()
    {
        bool nearWall = Physics2D.Raycast(RamblingCheck.position
            , Vector2.right * FaceDirection
            , playerData.ramblingCheckDistance
            , playerData.Rambler);
        return nearWall;
    }

    public bool CheckIfWalledBack()
    {
        bool nearBackWall = Physics2D.Raycast(WalledCheck.position, Vector2.right * -FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
        return nearBackWall;
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationTriggerFinished()
    {
        StateMachine.CurrentState.AnimationTriggerFinished();
    }

    public void CheckFlipFace(int xInput)
    {
        // Check if the player needs to flip face direction based on the X input.
        if (xInput != 0 && xInput != FaceDirection)
            FlipFace(); 
    }

    private void FlipFace()
    {
        // Flip the face direction of the player.
        FaceDirection *= -1; 
        transform.Rotate(0f, 180f, 0f); 
        cameraFollowObject.CallTurn(); 
    }
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(weaponBoundry.position, playerData.weaponBoundryRadius, playerData.Enemies))
        {
            gameManager.DestroyEnemy(collider.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = weaponBoundry == null ? Vector3.zero : weaponBoundry.position;
        Gizmos.DrawWireSphere(position, playerData.weaponBoundryRadius);
    }
}
