using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public bool CanMove = true;
    public float movementVelocity = 10;

    [Header("Jump State")]
    public float jumpVelocity = 15;
    public int amountOfJumps = 1;

    [Header("Air State")]
    public bool EdgeSticky = false;
    public float EdgeStickyJump = 0.2f;
    public float JumpHeightMult = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    
    [Header("Wall Climb State")]
    public LayerMask Walls;
    public bool wallClimb = false;
    public float wallClimbVelocity = 3f;

    [Header("Wall Jump State")]
    public bool wallJump = false;
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Weapon States")]
    public LayerMask Enemies;
    public float weaponBoundryRadius;
    public float weaponTime = 0.3f;

    [Header("Player Check States")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 1f;
    public float ramblingCheckDistance = 1f;
    public LayerMask GroundFloor;
    public LayerMask Rambler;

    [Header("Audio")]
    public AudioClip Walk;
    public AudioClip Jump;
    public AudioClip Sword;
    public AudioClip Throw;
    public AudioClip Respawn;
    public AudioClip Damage;
}
