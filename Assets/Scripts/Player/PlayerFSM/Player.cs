using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    #region Audio Variables

    public enum AudioFile
    {
        Walk,
        Jump,
        Sword,
        Throw,
        Respawn,
        Damage,
        LostLife
    }

    [Header("Audio")]
    [SerializeField]
    private AudioClip Walk;
    [SerializeField]
    private AudioClip Jump;
    [SerializeField]
    private AudioClip Sword;
    [SerializeField]
    private AudioClip Throw;
    [SerializeField]
    private AudioClip Respawn;
    [SerializeField]
    private AudioClip Damage;
    [SerializeField]
    private AudioClip LostLife;

    #endregion 

    #region Variables

    private CameraFollowObject cameraFollowObject;

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }
    public int FaceDirection { get; private set; }

    [Header("Others")]
    [SerializeField]
    private PlayerData playerData;

    private Vector2 workspace;

    private float fallSpeedYDampingChangeThrehold;

    private AudioSource audioSource;

    #endregion

    #region FSM State

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

    #endregion

    #region Transform Variable

    [SerializeField]
    private Transform GroundedCheck;
    [SerializeField]
    private Transform WalledCheck;

    #endregion

    #region FoundationFunction

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
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
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        cameraFollowObject = GameObject.FindGameObjectWithTag("CinaCamera").GetComponent<CameraFollowObject>();

        StateMachine.Initialize(IdleState);

        FaceDirection = 1;

        fallSpeedYDampingChangeThrehold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CurrentVelocity = rb.velocity;
        StateMachine.CurrentState.LogicUpdate();

        //if we are falling past a certain speed threshold
        if (rb.velocity.y < fallSpeedYDampingChangeThrehold
            && !CameraManager.instance.IsLerpingYDamping
            && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        //if we are standing still or moving up
        if (rb.velocity.y >= 0f
            && !CameraManager.instance.IsLerpingYDamping
            && CameraManager.instance.LerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion 

    public void PlaySound(AudioFile audioFile)
    {
        AudioClip selectedAudio = null;

        switch (audioFile)
        {
            case AudioFile.Walk:
                selectedAudio = Walk;
                break;
            case AudioFile.Jump:
                selectedAudio = Jump;
                break;
            case AudioFile.Sword:
                selectedAudio = Sword;
                break;
            case AudioFile.Throw:
                selectedAudio = Throw;
                break;
            case AudioFile.Respawn:
                selectedAudio = Respawn;
                break;
            case AudioFile.Damage:
                selectedAudio = Damage;
                break;
            case AudioFile.LostLife:
                selectedAudio = LostLife;
                break;
        }

        if (selectedAudio != null)
        {
            audioSource.PlayOneShot(selectedAudio);
        }
    }

    public void SetVolcityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVolcityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 Angle, int direction)
    {
        Angle.Normalize();
        workspace.Set(Angle.x * velocity * direction, Angle.y * velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(GroundedCheck.position, playerData.groundCheckRadius, playerData.GroundFloor);
    }

    public bool CheckIfWalled()
    {
        return Physics2D.Raycast(WalledCheck.position, Vector2.right * FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
    }

    public bool CheckIfWalledBack()
    {
        return Physics2D.Raycast(WalledCheck.position, Vector2.right * -FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
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
        if (xInput != 0 && xInput != FaceDirection)
            FlipFace();
    }

    private void FlipFace()
    {
        FaceDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
        cameraFollowObject.CallTurn();
    }
}
