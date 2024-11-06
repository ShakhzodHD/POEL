using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _close;
    private void Start()
    {
        _close.onClick.AddListener(OnButtonCloseClick);
    }
    private void OnButtonCloseClick()
    {
        var gameState = Boostrap.Instance.GameState;
        var uiManager = Boostrap.Instance.UIManager;

        switch (gameState)
        {
            case GameStates.InMenu:
                uiManager.ChangeMenuState(MenuStates.StartMenu);
                break;
            case GameStates.InProgress:
                uiManager.ChangeMenuState(MenuStates.Pause);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
        }
    }
}
