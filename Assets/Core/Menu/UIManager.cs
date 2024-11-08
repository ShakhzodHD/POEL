using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    [SerializeField] private List<GameObject> panels;

    private MenuStates _menuStates = MenuStates.StartMenu;

    public void Init()
    {
        Boostrap.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnGameStateChanged(GameStates gameStates)
    {
        switch (gameStates)
        {
            case GameStates.InMenu:
                ChangeMenuState(MenuStates.StartMenu);
                Boostrap.Instance.ScenesService.LoadMenu();
                break;
            case GameStates.InProgress:
                ChangeMenuState(MenuStates.Gameplay);
                panels[(int)MenuStates.Gameplay].GetComponent<GameplayPanel>().Init();
                break;
            case GameStates.GameOver:
                ChangeMenuState(MenuStates.GameOver);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameStates), gameStates, null);
        }
    }
    public void ChangeMenuState(MenuStates menuState)
    {
        _menuStates = menuState;
        OpenPanelForCurrentState();
    }
    private void OpenPanelForCurrentState()
    {
        panels.FirstOrDefault(panel => panel.activeSelf)?.SetActive(false);
        panels[(int)_menuStates].SetActive(true);
    }
}
