using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Text skillPointsText;
    [SerializeField] private Button close;

    private SkillButton[] skillButtons;
    private Character currentCharacter;
    private Player currentPlayer;
    private void Start()
    {
        close.onClick.AddListener(OnButtonCloseClick);
        Boostrap.Instance.ExperienceSystem.OnLevelUp += (newLevel) => RefreshUI();
    }

    private void OnButtonCloseClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Gameplay);
    }

    public void Initialize(Player player)
    {
        Debug.Log("Инициализация Upgrade Panel");
        currentPlayer = player;
        currentCharacter = Boostrap.Instance.PlayerData.characters[
            Boostrap.Instance.PlayerData.idSelectedCharacter];

        skillButtons = GetComponentsInChildren<SkillButton>();

        foreach (var button in skillButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(() => OnSkillButtonClicked(button.Skill));
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        skillPointsText.text = $"Skill Points: {currentCharacter.SkillPoints}";

        foreach (var button in skillButtons)
        {
            button.UpdateState(currentCharacter);
        }
    }

    private void OnSkillButtonClicked(Skill skill)
    {
        if (currentCharacter.UnlockSkill(skill))
        {
            currentPlayer.ApplySkillEffects(skill);
            RefreshUI();
        }
    }
}
