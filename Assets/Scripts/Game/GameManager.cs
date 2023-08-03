﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private bool musicLooping;
    private AudioSource audioSource;

    private GameObject gameOverMenu;

    public bool IsGamePaused { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        IsGamePaused = false;
        pauseMenu = GameObject.Find("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        audioSource = Camera.main.GetComponent<AudioSource>();
        musicLooping = false;

        SetupHuD();
    }

    private void SetupHuD()
    {
        TextMeshProUGUI scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        scoringDisplay.text = gameData.Score.ToString();

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

    private void ReduceOneLife()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (!musicLooping)
        {
            audioSource.clip = gameData.NormalState;
            audioSource.loop = true;
            audioSource.Play();

            musicLooping = true;
        }
    }

    private void RespawnPlayer()
    {
        player.transform.position = respawn.transform.position;
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        player.PlaySound(Player.AudioFile.LostLife);

        gameData.CurrentLives--;
        Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
        Destroy(heartToDelete.gameObject);

        if (gameData.CurrentLives == 0)
        {
            GameOver();
            return;
        }

        RespawnPlayer();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        if (Time.timeScale != 0f)
            Time.timeScale = 0f;

        ShowMenu("Canvas", true);
    }

    public void RetryGame()
    {
        ShowMenu("Canvas", false);
        SetupHuD();
        RespawnPlayer();

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
