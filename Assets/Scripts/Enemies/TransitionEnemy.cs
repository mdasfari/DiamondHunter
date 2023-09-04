using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TransitionEnemy : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    protected GameManager gameManager;

    [Header("Movement")]
    public float travelSpeed;
    public bool canPatrol;
    public float patrolDistance;
    public MoveDirections firstLocation;
    public MoveDirections lastLocation;

    protected float distanceTraveled = 0f;

    protected MoveDirections direction;
    protected Rigidbody2D rb;

    protected bool isAtThePost;
    protected bool isChasing;
    protected Vector3 originalPosition;
    protected Vector3 newTravelLocation;

    private Animator animator;

    protected Transform player;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        direction = firstLocation;
        isChasing = false;
        isAtThePost = true;
    }

    protected void FixedUpdate()
    {
        float measuredSpeed = travelSpeed;
        if (!isChasing)
        {
            if (isAtThePost)
            {
                if (!canPatrol)
                    return;

                newTravelLocation = GlobalFunctions.GetVectorDirection(direction);
                if (direction == firstLocation)
                    distanceTraveled++;
                else
                    distanceTraveled--;

                if (Mathf.Abs(distanceTraveled) >= patrolDistance)
                {
                    if (direction == firstLocation)
                        direction = lastLocation;
                    else
                        direction = firstLocation;
                }
            }
            else
            {
                measuredSpeed = travelSpeed * 4;
                newTravelLocation = originalPosition - transform.position;
                if (GlobalFunctions.IsVectorNearBy(originalPosition, transform.position))
                {
                    animator.SetBool("fly", false);
                    distanceTraveled = 0;
                    direction = firstLocation;
                    isAtThePost = true;
                }
            }
        }
        else
        {
            newTravelLocation = (player.position - transform.position);
            measuredSpeed = travelSpeed * 3;
        }

        rb.velocity = newTravelLocation.normalized * measuredSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            gameManager.PlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            player = collision.gameObject.transform;
            animator.SetBool("fly", true);
            isChasing = true;
            isAtThePost = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
            isChasing = false;
    }

}
