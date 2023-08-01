using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
