using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameData", menuName = "Data/Game Data/Base Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game States")]
    public AudioClip NormalState;
    public AudioClip ChasingState;
    public AudioClip GameOverState;

    [Header("Effectis")]
    public AudioClip OceanWaves;

    [Header("Game Settings")]
    public int StartupLives = 3;
    public int CurrentLives = 3;

    public int Score;

}
