using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameDataStore : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    [SerializeField]
    private PlayerData playerData;

    internal int CurrentLives = 3;
    internal int AddedLives = 0;
    internal int GameScore = 0;
    internal int NumberOfJump = 1;
    internal bool Gemstone = false;
    internal bool Nicklace = false;
    internal bool WallClimb = false;
    internal bool WallJump = false;
    internal bool EdgeSticky = false;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        Debug.Log("CurrentLives " + CurrentLives.ToString());
        Debug.Log("AddedLives " + AddedLives.ToString());
        Debug.Log("GameScore " + GameScore.ToString());
        Debug.Log("NumberOfJump " + NumberOfJump.ToString());
        Debug.Log("Gemstone " + Gemstone.ToString());
        Debug.Log("Nicklace " + Nicklace.ToString());
        Debug.Log("WallClimb " + WallClimb.ToString());
        Debug.Log("WallJump " + WallJump.ToString());
        Debug.Log("EdgeSticky " + EdgeSticky.ToString());
    }

    internal void newGame()
    {
        GameScore = 0;
        Gemstone = false;
        Nicklace = false;
        CurrentLives = gameData.StartupLives;
        AddedLives = 0;
        NumberOfJump = 1;
        WallClimb = false;
        WallJump = false;
        EdgeSticky = false;
    }
}
