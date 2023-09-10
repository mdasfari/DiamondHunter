using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private CollectableTypes CollectableType;

    private void Start()
    {
        if (CollectableType != CollectableTypes.Coin)
            gameObject.SetActive(!gameManager.IsCollectableAcquired(CollectableType));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.Collect(CollectableType);
            Destroy(gameObject);
        }
    }
}
