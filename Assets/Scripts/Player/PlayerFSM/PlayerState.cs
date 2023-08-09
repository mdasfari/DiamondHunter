using UnityEngine;

public class PlayerState
{
    protected Player player; // Reference to the Player object.
    protected PlayerStateMachine stateMachine; // Reference to the Player's state machine.
    protected PlayerData playerData; // Reference to the Player's data.

    protected bool isAnimationFinished; // Flag to check if the animation is finished.
    protected bool isExistingState; // Flag to check if the state is exiting.

    protected float startTime; // Start time for the state.

    private string animBoolName; // Animation boolean name used in Animator.

    // Constructor to initialize the PlayerState with required references and animation boolean name.
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    // Virtual method to enter the state, can be overridden by derived classes.
    public virtual void Enter()
    {
        DoChecks(); // Perform checks before entering the state.
        player.Anim.SetBool(animBoolName, true); // Set the animation boolean to true.
        startTime = Time.time; // Record the start time.
        isAnimationFinished = false; // Reset the animation finished flag.
        isExistingState = false; // Reset the existing state flag.
    }

    // Virtual method to exit the state, can be overridden by derived classes.
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false); // Set the animation boolean to false.
        isExistingState = true; // Mark the state as exiting.
    }

    // Virtual method for logic update, can be overridden by derived classes.
    public virtual void LogicUpdate()
    {

    }

    // Virtual method for physics update, can be overridden by derived classes.
    public virtual void PhysicsUpdate()
    {
        DoChecks(); // Perform checks during physics update.
    }

    // Virtual method for performing checks, can be overridden by derived classes.
    public virtual void DoChecks()
    {

    }

    // Virtual method for handling animation triggers, can be overridden by derived classes.
    public virtual void AnimationTrigger()
    {

    }

    // Virtual method for handling animation trigger finished, can be overridden by derived classes.
    public virtual void AnimationTriggerFinished()
    {
        isAnimationFinished = true; // Mark the animation as finished.
    }
}
