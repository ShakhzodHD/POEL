using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private Color availableColor = Color.yellow;
    [SerializeField] private Color lockedColor = Color.red;

    public Skill Skill;
    private Button button;
    public void UpdateState(Character character)
    {
        if (button == null) button = GetComponent<Button>();

        bool isUnlocked = character.UnlockedSkills.Contains(Skill);
        bool canUnlock = character.CanUnlockSkill(Skill);

        if (isUnlocked)
        {
            backgroundImage.color = unlockedColor;
            button.interactable = false;
        }
        else if (canUnlock)
        {
            backgroundImage.color = availableColor;
            button.interactable = true;
        }
        else
        {
            backgroundImage.color = lockedColor;
            button.interactable = false;
        }
    }
}
