using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    [Header("UI")]
    public Transform PauseMenu;

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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        musicLooping = false;

        SetupHuD();
    }

    private void SetupHuD()
    {
        TextMeshProUGUI scoringDisplay = ScoringObject.GetComponent<TextMeshProUGUI>();

        scoringDisplay.text = gameData.Score.ToString();

        for (int i = 0; i < gameData.CurrentLives; i++)
        {
            GameObject newHeart = Instantiate(HeartObject);
            newHeart.transform.SetParent(HeartContainer.transform);
        }
    }

    private void ReduceOneLife()
    {
        gameData.CurrentLives--;
        if (gameData.CurrentLives == 0)
        {
            // Game Over
        }

        Transform heartToDelete = HeartContainer.transform.GetChild(HeartContainer.transform.childCount - 1);
        Debug.Log(heartToDelete.name);
        Destroy(heartToDelete.gameObject);
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

    public void PlayerDead()
    {
        Time.timeScale = 0f;
        player.PlayLostLifeAudio();

        ReduceOneLife();

        player.transform.position = respawn.position;
        Time.timeScale = 1f;
    }
}
