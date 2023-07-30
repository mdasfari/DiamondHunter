using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    #region Variables
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }
    public int FaceDirection { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    private Vector2 workspace;

    #endregion

    #region FSM State

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerLandState LandState { get; private set; }


    public PlayerWallSlideState WallSlideState{ get; private set; }
    public PlayerWallGrapState WallGrapState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }

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
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "jump");
        AirState = new PlayerAirState(this, StateMachine, playerData, "air");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrapState = new PlayerWallGrapState(this, StateMachine, playerData, "wallGrap");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        StateMachine.Initialize(IdleState);

        FaceDirection = 1;
    }

    private void Update()
    {
        CurrentVelocity = rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion 

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

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(GroundedCheck.position, playerData.groundCheckRadius, playerData.GroundFloor);
    }

    public bool CheckIfWalled()
    {
        return Physics2D.Raycast(WalledCheck.position, Vector2.right * FaceDirection, playerData.wallCheckDistance, playerData.GroundFloor);
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
    }

}
