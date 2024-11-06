using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _menu;
    private void Start()
    {
        _play.onClick.AddListener(OnButtonPlayClick);
        _settings.onClick.AddListener(OnButtonSettingsClick);
        _menu.onClick.AddListener(OnButtonMenuClick);
    }
    private void OnButtonPlayClick()
    {
        Time.timeScale = 1f;
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Gameplay);
    }
    private void OnButtonSettingsClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Settings);
    }
    private void OnButtonMenuClick()
    {
        Boostrap.Instance.ChangeGameState(GameStates.InMenu);

    }
}
