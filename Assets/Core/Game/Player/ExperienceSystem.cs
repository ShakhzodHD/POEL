using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    public int ExperienceToNextLevel => Mathf.FloorToInt(100 * Mathf.Pow(currentLevel, 1.5f)); // Формула опыта

    private int currentExperience;
    private int currentLevel;

    public delegate void OnLevelUpHandler(int newLevel);
    public event OnLevelUpHandler OnLevelUp;

    public delegate void OnExperienceGainedHandler(int currentExperience, int experienceToNextLevel); //For UI
    public event OnExperienceGainedHandler OnExperienceGained;
    private void Awake()
    {
        Boostrap.Instance.ExperienceSystem = this;
    }
    public void Initialize()
    {
        Debug.Log("Инициализация ExperienceSystem");
        currentExperience = Boostrap.Instance.PlayerData.characters[Boostrap.Instance.PlayerData.idSelectedCharacter].CurrentExperience;
        currentLevel = Boostrap.Instance.PlayerData.characters[Boostrap.Instance.PlayerData.idSelectedCharacter].Level;
    }
    public void AddExperience(int amout)
    {
        currentExperience += amout;

        while (currentExperience >= ExperienceToNextLevel)
        {
            currentExperience -= ExperienceToNextLevel;
            LevelUp();
        }

        OnExperienceGained?.Invoke(currentExperience, ExperienceToNextLevel);
    }
    private void LevelUp()
    {
        currentLevel++;
        OnLevelUp?.Invoke(currentLevel);
    }
}
