using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    private bool musicLooping;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        musicLooping = false;
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
}
