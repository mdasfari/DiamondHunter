using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    [Header("UI")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;

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

    private AudioSource bgmAudioSource;
    private AudioSource effectsAudioSource;

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
        //pauseMenu = GameObject.Find("PauseMenu");
        //gameOverMenu = GameObject.Find("GameOverMenu");

        // AssignAudioSources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        bgmAudioSource = audioSources[0];
        effectsAudioSource = audioSources[1];

        scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        SetupHuD();
        SetupBGM();
        ChangeGameState(GameStates.Normal);
    }

    private void SetupBGM()
    {
        switch (gameData.GameLevel)
        {
            case GameLevels.Beach:
                gameData.NormalState = gameData.BeachBGM;
                break;
            case GameLevels.Ruine:
                gameData.NormalState = gameData.RuinsBGM;
                break;
        }
    }

    private void ChangeGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Normal:
                bgmAudioSource.clip = gameData.NormalState;
                bgmAudioSource.loop = true;
                break;
            case GameStates.Chase:
                bgmAudioSource.clip = gameData.ChasingState;
                bgmAudioSource.loop = true;
                break;
            case GameStates.GameOver:
                bgmAudioSource.clip = gameData.GameOverState;
                bgmAudioSource.loop = false;
                break;
        }

        bgmAudioSource.Play();
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

    public void CollectCoin()
    {
        effectsAudioSource.PlayOneShot(gameData.TreasureCollectionAudio);
        AddScore(gameData.Coin);
    }

    private void AddScore(int Score)
    {
        gameData.Score += Score;
        UpdateScore();
    }

    private void RespawnPlayer()
    {
        player.transform.position = respawn.transform.position;
    }

    public void SetRespawnLocation(UnityEngine.Vector3 newPsotion)
    {
        if (respawn.transform.position == newPsotion)
            return;

        effectsAudioSource.PlayOneShot(gameData.CheckpointAudio);
        respawn.transform.position = newPsotion;
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;

        if (gameData.CurrentLives > 0)
            gameData.CurrentLives--;
        else
            gameData.CurrentLives = 0;

        if (HeartContainer.transform.childCount > 0)
        {
            Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
            Destroy(heartToDelete.gameObject);
        }

        if (gameData.CurrentLives == 0)
        {
            GameOver();
            return;
        }

        effectsAudioSource.PlayOneShot(gameData.LostLife);
        RespawnPlayer();
        Time.timeScale = 1;
    }

    public void DestroyEnemy(GameObject enemy)
    {
        int newScore = 0;

        if (gameData.EnemyList.ContainsKey(enemy.tag))
        {
            newScore = gameData.EnemyList[enemy.tag];
            Debug.Log(string.Format("{0} score: {1}", enemy.tag, newScore));
        }

        AddScore(newScore);
        effectsAudioSource.PlayOneShot(gameData.EnemyKillAudio);
        Destroy(enemy);
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

    internal void LoadNextLevel(string sceneName, bool withAudio)
    {
        if (withAudio)
        {
            effectsAudioSource.PlayOneShot(gameData.GoalAudio);
            StartCoroutine(WaitForSound(effectsAudioSource, sceneName));
            player.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    IEnumerator WaitForSound(AudioSource source, string sceneName)
    {
        // Wait until sound has finished playing
        while (source.isPlaying)
        {
            yield return null;
        }

        // Audio has finished playing
        SceneManager.LoadScene(sceneName);
    }
}
