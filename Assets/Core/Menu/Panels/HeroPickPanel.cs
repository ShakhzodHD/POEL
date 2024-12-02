using UnityEngine;
using UnityEngine.UI;

public class HeroPickPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] heroSlots;
    [SerializeField] private Button[] deleteButtons;
    [SerializeField] private Button createHero;
    [SerializeField] private Button menu;
    private void Start()
    {
        createHero.onClick.AddListener(OnButtonCreateHeroClick);
        menu.onClick.AddListener(OnButtonMenuClick);

        for (int i = 0; i < deleteButtons.Length; i++)
        {
            int index = i;
            deleteButtons[i].onClick.AddListener(() => OnDeleteCharacterClick(index));
        }
    }
    private void OnEnable()
    {
        UpdateHeroSlots();
    }
    private void UpdateHeroSlots()
    {
        var characters = Boostrap.Instance.PlayerData.characters;

        for (int i = 0; i < heroSlots.Length; i++)
        {
            if (i < characters.Count)
            {
                var character = characters[i];
                heroSlots[i].GetComponentInChildren<Text>().text = character.Name;
                heroSlots[i].onClick.RemoveAllListeners();
                heroSlots[i].onClick.AddListener(() => StartGameWithCharacter(character));

                deleteButtons[i].gameObject.SetActive(true);
            }
            else
            {
                heroSlots[i].GetComponentInChildren<Text>().text = " ";
                heroSlots[i].onClick.RemoveAllListeners();

                deleteButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void StartGameWithCharacter(Character character)
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Gameplay);

        Boostrap.Instance.ChangeGameState(GameStates.InProgress);
        Boostrap.Instance.ScenesService.InitialLoad();
        Boostrap.Instance.ScenesService.LoadLevel(Constants.HIDEOUT_SCENE_NAME);
        Boostrap.Instance.GameEvents.OnEnterHideout?.Invoke();
        Boostrap.Instance.PlayerData.selectedCharacter = character;
    }
    private void OnButtonCreateHeroClick()
    {
        if (Boostrap.Instance.PlayerData.characters.Count >= heroSlots.Length)
        {
            Debug.Log("Все слоты заполнены. Удалите одного персонажа перед созданием нового.");
            return;
        }

        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroCreator);
    }
    private void OnButtonMenuClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.StartMenu);
    }
    private void OnDeleteCharacterClick(int index)
    {
        var characters = Boostrap.Instance.PlayerData.characters;

        if (index < characters.Count)
        {
            characters.RemoveAt(index);
            UpdateHeroSlots();
        }
    }
}
