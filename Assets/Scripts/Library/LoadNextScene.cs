using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string NextSceneName; // Name of the next scene to be loaded.

    // Start is called before the first frame update.
    void Start()
    {
        SceneManager.LoadScene(NextSceneName); // Load the specified scene by its name.
    }

    // Update is called once per frame.
    // Currently empty, but can be used for any per-frame logic if needed.
    void Update()
    {
        
    }
}
