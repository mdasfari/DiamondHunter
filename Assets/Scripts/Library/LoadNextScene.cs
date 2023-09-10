using UnityEngine;


public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private string NextSceneName; 

   
    void Start()
    {
        gameManager.LoadNextLevel(NextSceneName, false); 
    }
}
