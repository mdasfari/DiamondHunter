using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable] public class EnemyScoring : SerializableDictionary<string, int> { }


[CustomPropertyDrawer(typeof(EnemyScoring))]
public class MyDictionaryDrawer1 : DictionaryDrawer<string, int> { }


[CreateAssetMenu(fileName = "newGameData", menuName = "Data/Game Data/Base Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game States Audio")]
    public AudioClip NormalState; 
    public AudioClip ChasingState; 
    public AudioClip GameOverState;

    [Header("Game Level Background Music")]
    public AudioClip BeachBGM;
    public AudioClip RuinsBGM;

    [Header("Game Objects Audio")]
    public AudioClip CheckpointAudio;
    public AudioClip PowerupAudio;
    public AudioClip GoalAudio;
    public AudioClip TreasureCollectionAudio;
    public AudioClip EnemyKillAudio;
    public AudioClip LostLife;

    [Header("Effects")]
    public AudioClip OceanWaves; 

    [Header("Game Settings")]
    public int StartupLives = 3; 
    public int CurrentLives = 3;



    [Header("Game Scoring")]
    public GameLevels GameLevel;
    public int Score;

    public int Coin = 1;
    public EnemyScoring EnemyList = new EnemyScoring();
}
