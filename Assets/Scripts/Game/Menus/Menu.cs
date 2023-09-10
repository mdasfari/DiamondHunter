using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private GameManager gameManager;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NewGame()
    {
        PlayLocalSound();
        gameManager.StartNewGame();
    }

    public void QuitGame()
    {
        PlayLocalSound();
        Application.Quit();
    }

    public void ResumeGame()
    {
        PlayLocalSound();
        gameManager.ResumeGame();
    }

    public void QuitToMainMenu()
    {
        PlayLocalSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryGame()
    {
        PlayLocalSound(); 
        gameManager.RetryGame(); 
    }

    public void Exit()
    {
        PlayLocalSound();
        Application.Quit();
    }

    private IEnumerator PlayLocalSound()
    {
        audioSource.Play();
        yield return StartCoroutine(WaitForSound(audioSource));
    }

    IEnumerator WaitForSound(AudioSource source)
    {
        // Wait until sound has finished playing
        while (source.isPlaying)
        {
            yield return null;
        }

        // Audio has finished playing
        yield return true;
    }
}
