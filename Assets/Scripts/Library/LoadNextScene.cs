using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string NextSceneName; 

   
    void Start()
    {
        SceneManager.LoadScene(NextSceneName); 
    }

    
    
    void Update()
    {
        
    }
}
