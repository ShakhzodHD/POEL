using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateHeroPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _heroPick;
    [SerializeField] private Button _create;

    private void Start()
    {
        _heroPick.onClick.AddListener(OnButtonHeroPickClick);
        _create.onClick.AddListener(OnButtonCreateClick);
    }

    private void OnButtonHeroPickClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroPick);
    }
    private void OnButtonCreateClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroPick);

        //реализация создания персонажа
    }
}
