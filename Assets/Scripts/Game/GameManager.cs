using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Game States Audio")]
    public AudioClip NormalState;
    public AudioClip ChasingState;
    public AudioClip GameOverState;

    [Header("Game Data")]
    [SerializeField]
    internal GameLevels GameLevel;

    [SerializeField]
    private GameDataStore gameDataStore;

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

    // Start is called before the first frame update
    void Start()
    {
        // AssignAudioSources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Count() > 0)
        {
            bgmAudioSource = audioSources[0];
            effectsAudioSource = audioSources[1];
        }

        //gameDataStore = store.GetComponent<GameDataStore>();

        if (ScoringObject)
            scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        IsGamePaused = false;
        SetupBGM();
        SetupHuD();

        ChangeGameState(GameStates.Normal);
    }

    public void StartNewGame()
    {
        gameDataStore.newGame();
        SceneManager.LoadScene("BeachIntro");
    }

    public bool IsGamePaused { get; private set; }

    private void SetupBGM()
    {
        Debug.Log(GameLevel);
        switch (GameLevel)
        {
            case GameLevels.MainMenu:
                NormalState = gameData.MainMenuBGM;
                break;
            case GameLevels.Beach:
                NormalState = gameData.BeachBGM;
                break;
            case GameLevels.Ruine:
                NormalState = gameData.RuinsBGM;
                break;
            case GameLevels.Win:
                NormalState = gameData.WinGameBGM;
                break;
            case GameLevels.Lose:
                NormalState = gameData.LoseGameBGM;
                break;
        }
    }

    private void ChangeGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Normal:
                bgmAudioSource.clip = NormalState;
                bgmAudioSource.loop = true;
                break;
            case GameStates.Chase:
                bgmAudioSource.clip = ChasingState;
                bgmAudioSource.loop = true;
                break;
            case GameStates.GameOver:
                bgmAudioSource.clip = GameOverState;
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

        for (int i = 0; i < gameDataStore.CurrentLives; i++)
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
            scoringDisplay.text = gameDataStore.GameScore.ToString();
    }

    public void Collect(CollectableTypes type)
    {
        effectsAudioSource.PlayOneShot(gameData.TreasureCollectionAudio);
        switch (type)
        {
            case CollectableTypes.Coin:
                AddScore(gameData.CoinScoreValue);
                break;
            case CollectableTypes.Gemstone:
                AddScore(gameData.TreasureScoreValue);
                gameDataStore.Gemstone = true;
                break;
            case CollectableTypes.Nicklace:
                AddScore(gameData.TreasureScoreValue);
                gameDataStore.Nicklace = true;
                break;
            case CollectableTypes.DoubleJump:
                AddScore(gameData.PowerUpScoreValue);
                gameDataStore.NumberOfJump = 2;
                break;
            case CollectableTypes.WallGrab:
                AddScore(gameData.PowerUpScoreValue);
                gameDataStore.WallClimb = true;
                break;
        }
    }

    internal bool IsCollectableAcquired(CollectableTypes type)
    {
        bool result = false;

        switch (type)
        {
            case CollectableTypes.Gemstone:
                result = gameDataStore.Gemstone;
                break;
            case CollectableTypes.Nicklace:
                result = gameDataStore.Nicklace;
                break;
            case CollectableTypes.DoubleJump:

                result = gameDataStore.NumberOfJump > 1;
                break;
            case CollectableTypes.WallGrab:
                result = gameDataStore.WallClimb;
                break;
        }

        return result;
    }

    private void AddScore(int Score)
    {
        Score += Score;
        CheckForNewLife();
        UpdateScore();
    }

    private void CheckForNewLife()
    {
        float currentLiveIncrese = gameDataStore.GameScore / gameData.NewLiveRequiredScore;
        if (currentLiveIncrese > gameDataStore.AddedLives)
        {
            gameDataStore.AddedLives = (int)Math.Floor(currentLiveIncrese);
            gameDataStore.CurrentLives++;
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

        if (gameDataStore.CurrentLives > 0)
            gameDataStore.CurrentLives--;
        else
            gameDataStore.CurrentLives = 0;

        if (HeartContainer)
        {
            if (HeartContainer.transform.childCount > 0)
            {
                Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
                Destroy(heartToDelete.gameObject);
            }
        }

        if (gameDataStore.CurrentLives == 0)
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

        switch (enemy.tag)
        {
            case "Bee":
                newScore = gameData.BeeScoreValue;
                break;
            case "Bat":
                newScore = gameData.BatScoreValue;
                break;
            case "Skeleton":
                newScore = gameData.SkeletonScoreValue;
                break;
            case "Skull":
                newScore = gameData.SkullScoreValue;
                break;
            case "Slime":
                newScore = gameData.SlimeScoreValue;
                break;
            default:
                newScore = 0;
                break;
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
        gameDataStore.CurrentLives = gameData.StartupLives;

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
            /*
            switch (sceneName)
            {
                case "MainMenu":
                    GameLevel = GameLevels.MainMenu;
                    break;
                case "Beach":
                    GameLevel = GameLevels.Beach;
                    break;
                case "Ruine":
                    GameLevel = GameLevels.Ruine;
                    break;
                case "VictoryLost":
                    if (gameDataStore.Gemstone && gameDataStore.Nicklace)
                        gameDataStore.GameLevel = GameLevels.Win;
                    else
                        gameDataStore.GameLevel = GameLevels.Lose;
                    break;
            
            }
            */

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
