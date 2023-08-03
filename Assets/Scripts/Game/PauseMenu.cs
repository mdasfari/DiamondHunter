using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip SelectSound;
    // Start is called before the first frame update
    public void ResumeGame()
    {

    }

    public void QuitToMainMenu()
    {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.PlayOneShot(SelectSound);

        SceneManager.LoadScene("MainMenu");
    }
}
