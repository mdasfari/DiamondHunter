using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Game")]
    [SerializeField]
    private GameManager gameManager; // Reference to the GameManager to control game states.

    private AudioSource audioSource; // Reference to the AudioSource component to play sounds.

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject.
    }

    public void NewGame()
    {
        PlayLocalSound(); // Play click sound.
        gameManager.StartNewGame();
    }

    public void QuitGame()
    {
        PlayLocalSound(); // Play click sound.
        Application.Quit(); // Quit the application.
    }

    public void ResumeGame()
    {
        PlayLocalSound(); // Play click sound.
        gameManager.ResumeGame(); // Call the ResumeGame method from the GameManager to resume the game.
    }

    public void QuitToMainMenu()
    {
        PlayLocalSound(); // Play click sound.
        gameManager.ToMainMenu();
    }

    public void SettingsMenu()
    {
        PlayLocalSound(); // Play click sound.
        SceneManager.LoadScene("SettingsMenu"); // Load the SettingsMenu scene.
    }

    public void BackToMainMenu()
    {
        PlayLocalSound(); // Play click sound.
        SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene.
    }

    public void RetryGame()
    {
        PlayLocalSound(); // Play click sound.
        gameManager.RetryGame(); // Call the RetryGame method from the GameManager to retry the game.
    }

    public void Exit()
    {
        PlayLocalSound(); // Play click sound.
        Application.Quit(); // Quit the application.
    }

    private void PlayLocalSound()
    {
        if(audioSource)
            audioSource.Play(); // Play the sound attached to the AudioSource component.
    }
}
