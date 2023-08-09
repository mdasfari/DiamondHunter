using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importing the scene management namespace.

public class StartRuinsLevel : MonoBehaviour
{
    [SerializeField]
    private string SceneName; // Name of the scene to be loaded when the player collides with this object.

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Player".
        if (collision.gameObject.tag == "Player")
        {
            // Load the specified scene (Ruins level) when the player collides with this object.
            SceneManager.LoadScene(SceneName);
        }
    }
}
