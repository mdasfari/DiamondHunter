using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameData gameData;

    [Header("Audio")]
    [SerializeField]
    private AudioClip MenuSelectSound;
    
    public void NewGame()
    {
        PlayLocalSound();
        gameData.CurrentLives = gameData.StartupLives;
        SceneManager.LoadScene("BeachIntro");
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
        gameData.CurrentLives = gameData.StartupLives;
        gameManager.RetryGame();
    }

    public void Exit()
    {
        PlayLocalSound();
        Application.Quit();
    }

    private void PlayLocalSound()
    {
        if (MenuSelectSound != null)
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            audioSource.PlayOneShot(MenuSelectSound);
        }
    }
}
