﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    [Header("UI")]
    public GameObject pauseMenu;

    [Header("Game HUD")]
    [SerializeField]
    private GameObject HeartContainer;

    [SerializeField]
    private GameObject HeartObject;

    [SerializeField]
    private RectTransform ScoringObject;

    [Header("Player")]
    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform respawn;

    // Internal Variables
    private GameStates currentGameState;

    private AudioSource audioSource;
    private GameObject gameOverMenu;

    TextMeshProUGUI scoringDisplay;

    public void StartNewGame()
    {
        gameData.CurrentLives = gameData.StartupLives; 
        SceneManager.LoadScene("BeachIntro"); 
    }

    public bool IsGamePaused { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        IsGamePaused = false;
        pauseMenu = GameObject.Find("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        audioSource = GetComponent<AudioSource>();

        scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        SetupHuD();
        ChangeGameState(GameStates.Normal);
    }

    private void ChangeGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Normal:
                audioSource.clip = gameData.NormalState;
                audioSource.loop = true;
                break;
            case GameStates.Chase:
                audioSource.clip = gameData.ChasingState;
                audioSource.loop = true;
                break;
            case GameStates.GameOver:
                audioSource.clip = gameData.GameOverState;
                audioSource.loop = false;
                break;
        }

        audioSource.Play();
        currentGameState = newState;
    }

    private void SetupHuD()
    {
        UpdateScore();
        UpdateLives();
    }

    private void UpdateLives()
    {
        while (HeartContainer.transform.childCount > 0)
        {
            Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
            Destroy(heartToDelete.gameObject);
        }

        for (int i = 0; i < gameData.CurrentLives; i++)
        {
            GameObject newHeart = Instantiate(HeartObject);
            newHeart.transform.SetParent(HeartContainer.transform);
        }
    }

    private void UpdateScore()
    {
        scoringDisplay.text = gameData.Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int Score)
    {
        audioSource.PlayOneShot(gameData.TreasureCollectionAudio);
        gameData.Score += Score;
        UpdateScore();
    }

    private void RespawnPlayer()
    {
        player.transform.position = respawn.transform.position;
    }

    public void SetRespawnLocation(Vector3 newPsotion)
    {
        respawn.transform.position = newPsotion;
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;

        gameData.CurrentLives--;
        Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
        Destroy(heartToDelete.gameObject);

        if (gameData.CurrentLives == 0)
        {
            GameOver();
            return;
        }

        player.PlaySound(PlayerAudioFiles.LostLife);
        RespawnPlayer();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        if (Time.timeScale != 0f)
            Time.timeScale = 0f;

        ShowMenu("Canvas", true);
        ChangeGameState(GameStates.GameOver);
    }

    public void RetryGame()
    {
        gameData.CurrentLives = gameData.StartupLives;
        gameData.Score = 0;

        ShowMenu("Canvas", false);
        SetupHuD();
        RespawnPlayer();

        ChangeGameState(GameStates.Normal);

        if (Time.timeScale != 1f)
            Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.transform.Find("Panel").gameObject.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.gameObject.transform.Find("Panel").gameObject.SetActive(true);
        IsGamePaused = true;
    }
    private void ShowMenu(string menuName, bool show)
    {
        gameOverMenu.gameObject.transform.Find(menuName).gameObject.SetActive(show);
    }
}
