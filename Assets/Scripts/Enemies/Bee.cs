using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private GameManager gameManager;

    [Header("Movement")]
    public float Speed;
    public float PetrolDistance;

    private float distanceTraveled = 0f;

    private MoveDirections direction;
    private Rigidbody2D rb;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = MoveDirections.Up;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 directionVector = new Vector2();

        switch (direction)
        {
            case MoveDirections.Up:
                directionVector = Vector2.up;
                distanceTraveled++;
                break;
            case MoveDirections.Down:
                directionVector = Vector2.down;
                distanceTraveled--;
                break;
        }

        rb.velocity = directionVector.normalized * Speed;

        if (Mathf.Abs(distanceTraveled) >= PetrolDistance)
        {
            if (direction == MoveDirections.Up)
                direction = MoveDirections.Down;
            else
                direction = MoveDirections.Up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.PlayerDead();
        }
    }

}
