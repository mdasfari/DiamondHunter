using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameData", menuName = "Data/Game Data/Base Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game States")]
    public AudioClip NormalState;
    public AudioClip ChasingState;

    [Header("Effectis")]
    public AudioClip OceanWaves;

}
