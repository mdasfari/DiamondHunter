using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attribute to allow the creation of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "newGameData", menuName = "Data/Game Data/Base Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game States Audio")]
    public AudioClip NormalState; // Audio clip for the normal state of the game.
    public AudioClip ChasingState; // Audio clip for the chasing state of the game.
    public AudioClip GameOverState; // Audio clip for the game over state.

    public AudioClip TreasureCollectionAudio; // Audio clip for collecting treasures.

    [Header("Effects")]
    public AudioClip OceanWaves; // Audio clip for the ocean waves effect.

    [Header("Game Settings")]
    public int StartupLives = 3; // The initial number of lives when the game starts.
    public int CurrentLives = 3; // The current number of lives during gameplay.

    public int Score; // The current score of the game.
}
