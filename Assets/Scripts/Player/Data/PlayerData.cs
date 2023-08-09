using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public bool CanMove = true; // Determines if the player can move.
    public float movementVelocity = 10; // Movement speed of the player.

    [Header("Jump State")]
    public float jumpVelocity = 15; // Jumping speed of the player.
    public int amountOfJumps = 1; // Number of jumps the player can perform.

    [Header("Air State")]
    public float EdgeStickyJump = 0.2f; // Stickiness factor when jumping near edges.
    public float JumpHeightMult = 0.5f; // Multiplier for jump height.

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f; // Sliding speed when against a wall.

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f; // Climbing speed when on a wall.

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f; // Jumping speed when jumping off a wall.
    public float wallJumpTime = 0.4f; // Time duration for wall jump.
    public Vector2 wallJumpAngle = new Vector2(1, 2); // Angle for wall jump.

    [Header("Player Check States")]
    public float groundCheckRadius = 0.3f; // Radius for checking if the player is on the ground.
    public float wallCheckDistance = 1f; // Distance for checking if the player is near a wall.
    public LayerMask GroundFloor; // Layer mask for identifying ground floor.

    [Header("Audio")]
    public AudioClip Walk; // Audio clip for walking.
    public AudioClip Jump; // Audio clip for jumping.
    public AudioClip Sword; // Audio clip for sword attack.
    public AudioClip Throw; // Audio clip for throwing.
    public AudioClip Respawn; // Audio clip for respawning.
    public AudioClip Damage; // Audio clip for taking damage.
    public AudioClip LostLife; // Audio clip for losing a life.
}
