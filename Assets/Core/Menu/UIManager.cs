using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private List<GameObject> panels;

    [SerializeField] private RectTransform mainInventoryPanel;

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
                CloseInventory();
                Boostrap.Instance.ScenesService.LoadMenu();
                break;
            case GameStates.InProgress:
                ChangeMenuState(MenuStates.Gameplay);
                panels[(int)MenuStates.Gameplay].GetComponent<GameplayPanel>().Init();
                break;
            case GameStates.GameOver:
                ChangeMenuState(MenuStates.GameOver);
                Boostrap.Instance.PlayerData.input.SwitchCurrentActionMap("Disable");
                CloseInventory();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameStates), gameStates, null);
        }
    }
    public void ChangeMenuState(MenuStates menuState)
    {
        _menuStates = menuState;
        if (_menuStates == MenuStates.Pause) CloseInventory();
        OpenPanelForCurrentState();
    }
    private void OpenPanelForCurrentState()
    {
        panels.FirstOrDefault(panel => panel.activeSelf)?.SetActive(false);
        panels[(int)_menuStates].SetActive(true);
    }
    public void InitUpgradePanel(Player player)
    {
        panels[(int)MenuStates.Upgrade].GetComponent<UpgradePanel>().Initialize(player);
    }

    public void ToggleInventory()
    {
        if (mainInventoryPanel != null)
        {
            bool isActive = mainInventoryPanel.gameObject.activeSelf;
            mainInventoryPanel.gameObject.SetActive(!isActive);

            //Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            //Cursor.visible = !isActive;
        }
    }
    private void CloseInventory()
    {
        if (mainInventoryPanel != null) mainInventoryPanel.gameObject.SetActive(false);
    }
}
