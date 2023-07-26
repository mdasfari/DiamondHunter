using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreLibrary;

public enum PlayerStates
{
    Idle,
    Run,
    Spawn,
    Attack,
    Throw,
    Damage,
    Die
}

public class PlayerBehavior : MonoBehaviour
{
    

    private SpriteRenderer spriteRender;
    private Animator animator;
    private Rigidbody2D rb;
    public int PlayerSpeed = 7;
    public int PlayerJumpForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        bool jumpState = false;
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!!!!");
            // rb.AddForce(Vector2.up * PlayerJumpForce, ForceMode2D.Impulse);
            jumpState = true;
        }


        animator.SetBool("Jumping", jumpState);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), jumpState ? PlayerJumpForce : 0);

        if (moveInput.x > 0.01f)
            spriteRender.flipX = false;
        else if (moveInput.x < 0.01f)
            spriteRender.flipX = true;

        rb.velocity = moveInput * PlayerSpeed; // .normalized * PlayerSpeed;

        animator.SetBool("Running", (moveInput.x != 0));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
