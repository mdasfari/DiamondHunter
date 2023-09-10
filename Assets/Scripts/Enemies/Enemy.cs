﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    protected Transform player;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                    {
                        direction = lastLocation;
                        transform.Rotate(0f, 180f, 0f);
                    }
                    else
                    {
                        direction = firstLocation;
                        transform.Rotate(0f, 180f, 0f);
                    }
                }
            }
            else
            {
                measuredSpeed = travelSpeed * 4;
                newTravelLocation = originalPosition - transform.position;
                if (GlobalFunctions.IsVectorNearBy(originalPosition, transform.position))
                {
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
