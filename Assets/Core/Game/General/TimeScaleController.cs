using System;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    private void Start()
    {
        Boostrap.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
    public void ApplyTimeDilation()
    {
        Time.timeScale = 0.5f;
    }
    private void OnGameStateChanged(GameStates gameStates)
    {
        switch (gameStates)
        {
            case GameStates.InMenu:
                PauseGame();
                break;
            case GameStates.InProgress:
                UnpauseGame();
                break;
            case GameStates.GameOver:
                ApplyTimeDilation();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameStates), gameStates, null);
        }
    }
}
