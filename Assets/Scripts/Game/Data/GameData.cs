using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
[Serializable] public class EnemyScoring : SerializableDictionary<string, int> { }


[CustomPropertyDrawer(typeof(EnemyScoring))]
public class MyDictionaryDrawer1 : DictionaryDrawer<string, int> { }
*/

[CreateAssetMenu(fileName = "newGameData", menuName = "Data/Game Data/Base Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game Level Background Music")]
    public AudioClip MainMenuBGM;
    public AudioClip BeachBGM;
    public AudioClip RuinsBGM;

    public AudioClip WinGameBGM;
    public AudioClip LoseGameBGM;

    [Header("Game Objects Audio")]
    public AudioClip CheckpointAudio;
    public AudioClip PowerupAudio;
    public AudioClip GoalAudio;
    public AudioClip TreasureCollectionAudio;
    public AudioClip EnemyKillAudio;
    public AudioClip LostLife;
    public AudioClip NewLife;

    [Header("Effects")]
    public AudioClip OceanWaves; 

    [Header("Game Settings")]
    public int StartupLives = 3; 
    
    public int NewLiveRequiredScore = 200;

    // public EnemyScoring EnemyList = new EnemyScoring();
    public int CoinScoreValue = 1;
    public int PowerUpScoreValue = 5;
    public int TreasureScoreValue = 10;
    public int BatScoreValue = 5;
    public int BeeScoreValue = 5;
    public int SlimeScoreValue = 10;
    public int SkeletonScoreValue = 15;
    public int SkullScoreValue = 25;
}
