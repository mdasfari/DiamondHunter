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
    private GameLevels gameLevel;

    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private PlayerData playerData;
    

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
        gameData.Score = 0;
        gameData.Gemstone = false;
        gameData.Nicklace = false;
        gameData.CurrentLives = gameData.StartupLives;
        gameData.AddedLives = 0;

        playerData.amountOfJumps = 1;
        playerData.wallClimb = false;
        playerData.wallJump = false;
        playerData.EdgeSticky = false;

        SceneManager.LoadScene("BeachIntro");
    }

    public bool IsGamePaused { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        gameData.GameLevel = gameLevel;
        IsGamePaused = false;
        //pauseMenu = GameObject.Find("PauseMenu");
        //gameOverMenu = GameObject.Find("GameOverMenu");

        // AssignAudioSources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        bgmAudioSource = audioSources[0];
        effectsAudioSource = audioSources[1];

        if (ScoringObject)
            scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        SetupHuD();
        SetupBGM();
        ChangeGameState(GameStates.Normal);
    }

    private void SetupBGM()
    {
        switch (gameData.GameLevel)
        {
            case GameLevels.MainMenu:
                gameData.NormalState = gameData.MainMenuBGM;
                break;
            case GameLevels.Beach:
                gameData.NormalState = gameData.BeachBGM;
                break;
            case GameLevels.Ruine:
                gameData.NormalState = gameData.RuinsBGM;
                break;
            case GameLevels.Win:
                gameData.NormalState = gameData.WinGameBGM;
                break;
            case GameLevels.Lose:
                gameData.NormalState = gameData.LoseGameBGM;
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
        if (HeartContainer)
        {
            int hearCount = HeartContainer.transform.childCount;
            if (hearCount > 0)
            {
                for (int i = 0; i < hearCount; i++)
                {
                    Transform heartToDelete = HeartContainer.transform.GetChild(0);
                    Destroy(heartToDelete.gameObject);
                }
            }
        }

        for (int i = 0; i < gameData.CurrentLives; i++)
        {
            AddDisplayHeart();
        }
    }

    private void AddDisplayHeart()
    {
        if (HeartContainer)
        {
            GameObject newHeart = Instantiate(HeartObject);
            newHeart.transform.SetParent(HeartContainer.transform);
        }
    }

    private void UpdateScore()
    {
        if (scoringDisplay)
            scoringDisplay.text = gameData.Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(CollectableTypes type)
    {
        effectsAudioSource.PlayOneShot(gameData.TreasureCollectionAudio);
        switch(type)
        {
            case CollectableTypes.Coin:
                AddScore(gameData.CoinScoreValue);
                break;
            case CollectableTypes.Gemstone:
                AddScore(gameData.TreasureScoreValue);
                gameData.Gemstone = true;
                break;
            case CollectableTypes.Nicklace:
                AddScore(gameData.TreasureScoreValue);
                gameData.Nicklace = true;
                break;
            case CollectableTypes.DoubleJump:
                AddScore(gameData.PowerUpScoreValue);
                player.CollectDoubleJump();
                break;
            case CollectableTypes.WallGrab:
                AddScore(gameData.PowerUpScoreValue);
                player.CollectWallGrab();
                break;
        }
    }

    internal bool IsCollectableAcquired(CollectableTypes type)
    {
        bool result = false;

        switch (type)
        {
            case CollectableTypes.Gemstone:
                result = gameData.Gemstone;
                break;
            case CollectableTypes.Nicklace:
                result = gameData.Nicklace;
                break;
            case CollectableTypes.DoubleJump:
                
                player.isDoubleJump();
                break;
            case CollectableTypes.WallGrab:
                
                player.isCollectWallGrab();
                break;
        }

        return result;
    }

    private void AddScore(int Score)
    {
        gameData.Score += Score;
        CheckForNewLife();
        UpdateScore();
    }

    private void CheckForNewLife()
    {
        float currentLiveIncrese = gameData.Score / gameData.NewLiveRequiredScore;
        if (currentLiveIncrese > gameData.AddedLives)
        {
            gameData.AddedLives = (int)Math.Floor(currentLiveIncrese);
            gameData.CurrentLives++;
            AddDisplayHeart();
            effectsAudioSource.PlayOneShot(gameData.NewLife);
        }
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

        if (HeartContainer)
        {
            if (HeartContainer.transform.childCount > 0)
            {
                Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
                Destroy(heartToDelete.gameObject);
            }
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

        /*
        if (gameData.EnemyList.ContainsKey(enemy.tag))
        {
            newScore = gameData.EnemyList[enemy.tag];
            Debug.Log(string.Format("{0} score: {1}", enemy.tag, newScore));
        }
        */

        newScore = 100;

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
