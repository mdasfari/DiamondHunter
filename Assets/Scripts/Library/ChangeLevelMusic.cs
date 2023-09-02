using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelMusic : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameLevels level;
    // Start is called before the first frame update
    void Start()
    {
        gameData.GameLevel = level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
