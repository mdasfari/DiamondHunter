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


    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        audioSource.Play();
    }
}
