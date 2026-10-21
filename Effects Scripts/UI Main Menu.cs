using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _startGame;
    [SerializeField] Button _endGame;

    void Start()
    {
        _startGame.onClick.AddListener(StartGame);
        _endGame.onClick.AddListener(EndGame);
    }

    private void StartGame()
    {
        ScenesManager.sceneInstance.LoadNewGame();
    }

    private void EndGame()
    {
        ScenesManager.sceneInstance.ExitGame();
    }

}
