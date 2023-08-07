using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Treasure")]
    public int ObjectValue = 1;

    [Header("Game")]
    [SerializeField]
    private GameManager gameManager;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.AddScore(ObjectValue);
            Destroy(gameObject);
        }
    }
}
