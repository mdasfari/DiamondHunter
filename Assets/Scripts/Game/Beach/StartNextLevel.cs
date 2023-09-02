using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextLevel : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private string SceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.LoadNextLevel(SceneName, true);
        }
    }
}
