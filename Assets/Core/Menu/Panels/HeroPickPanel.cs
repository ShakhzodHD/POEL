using UnityEngine;
using UnityEngine.UI;

public class HeroPickPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _heroSlots;
    [SerializeField] private Button _createHero;
    [SerializeField] private Button _menu;
    private void Start()
    {
        _heroSlots[0].onClick.AddListener(OnButtonHeroClick);
        _createHero.onClick.AddListener(OnButtonCreateHeroClick);
        _menu.onClick.AddListener(OnButtonMenuClick);
    }
    private void OnButtonHeroClick()
    {
        //реализацию ждет
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Gameplay);

        Boostrap.Instance.ChangeGameState(GameStates.InProgress);
        Boostrap.Instance.ScenesService.InitialLoad();
        Boostrap.Instance.ScenesService.LoadLevel(Constants.HIDEOUT_SCENE_NAME);
        Boostrap.Instance.GameEvents.OnEnterHideout?.Invoke();
    }
    private void OnButtonCreateHeroClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroCreator);
    }
    private void OnButtonMenuClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.StartMenu);
    }
}
