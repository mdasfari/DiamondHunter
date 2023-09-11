using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoring : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private RectTransform ScoreDisplay;
    private TextMeshProUGUI scoringObject;

    [SerializeField]
    private Transform loseTileMap;
    [SerializeField]
    private Transform NewGameButton;
    [SerializeField]
    private Transform winTileMap;

    // Start is called before the first frame update
    void Start()
    {
        bool gameWon = (gameData.Gemstone && gameData.Nicklace);
        scoringObject = ScoreDisplay.GetComponent<TextMeshProUGUI>();
        scoringObject.text = gameData.Score.ToString();

            NewGameButton.gameObject.SetActive(!gameWon);
            loseTileMap.gameObject.SetActive(!gameWon);
            winTileMap.gameObject.SetActive(gameWon);
    }
}
