using System;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private ActiveAbility majorAbility;
    [SerializeField] private ActiveAbility minorAbility;
    [SerializeField] private ActiveAbility escapeAbility;
    [SerializeField] private PassiveAbility passiveAbility;

    private Character currentCharacter;
    private CharacterClass characterClass;
    private PlayerMovement playerMovement;

    private HealthSystem healthSystem;
    private ResourceSystem resourceSystem;

    private void Start()
    {
        Boostrap.Instance.TopDownCamera.SetTarget(gameObject.transform);

        playerMovement = GetComponent<PlayerMovement>();

        InitCharacter();
        Boostrap.Instance.UIManager.InitUpgradePanel(this);
        Boostrap.Instance.ExperienceSystem.Initialize();
        Boostrap.Instance.ExperienceSystem.OnExperienceGained += OnExperienceGained;
        Boostrap.Instance.ExperienceSystem.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp(int newLevel)
    {
        if (newLevel <= 0)
        {
            throw new ArgumentException("New level cannot be negative: " + newLevel);
        }

        currentCharacter.AddSkillPoints(1);
        currentCharacter.Level = newLevel;
        

        Debug.Log($"LevelUP! New Level: {newLevel}");
    }

    private void OnExperienceGained(int currentExperience, int experienceToNextLevel)
    {
        Debug.Log($"OnExperienceGained: CurrentExp {currentCharacter} CurrentExpToNextLevel {experienceToNextLevel}");
        currentCharacter.CurrentExperience = currentExperience;
        currentCharacter.ExperienceToNextLevel = experienceToNextLevel;
    }

    private void Update() // Временно
    {
        if (Input.GetKeyUp(KeyCode.T)) // Открыть панель c улучшениями
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Upgrade);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Boostrap.Instance.ExperienceSystem.AddExperience(50);
        }
    }
    private void InitCharacter()
    {
        int selectedIndex = Boostrap.Instance.PlayerData.idSelectedCharacter;
        var characters = Boostrap.Instance.PlayerData.characters;

        if (selectedIndex >= 0 && selectedIndex < characters.Count)
        {
            var character = characters[selectedIndex];
            currentCharacter = character;
            characterClass = character.CharacterClass;

            healthSystem = new HealthSystem(characterClass.baseHealth);
            resourceSystem = new ResourceSystem(characterClass.baseResource);

            playerMovement.MovementSpeed = character.Speed;

            Debug.Log("Макс здоровье: " + healthSystem.MaxHealth + " Текущее здоровье: " + healthSystem.CurrentHealth);
            Debug.Log("Макс ресурс: " + resourceSystem.MaxResource + " Текущи ресурс: " + resourceSystem.MaxResource);

            foreach (var stat in character.Stats.stats)
            {
                Debug.Log($"{stat.Key}: {stat.Value}");
            }

            majorAbility = character.MajorAbility;
            minorAbility = character.MinorAbility;
            escapeAbility = character.EscapeAbility;
            passiveAbility = character.PassiveAbility;

            passiveAbility.ApplyEffect(gameObject); 
        }
        else
        {
            Debug.LogError("Неверный индекс персонажа.");
        }
    }
    public void ApplySkillEffects(Skill skill)
    {
        skill.ApplyEffect(currentCharacter);
    }
    public void UseAbility(AbilitySlotType slotType)
    {
        switch (slotType)
        {
            case AbilitySlotType.Major:
                if (majorAbility != null)
                {
                    Debug.Log("Мажор");
                    if (CanUseAbility(majorAbility))
                        majorAbility.Activate(gameObject);
                    else { Debug.Log("Недостаточно ресурса"); }
                }
                break;

            case AbilitySlotType.Minor:
                if (minorAbility != null)
                {
                    Debug.Log("Минор");
                    if (CanUseAbility(minorAbility))
                        minorAbility.Activate(gameObject);
                }
                break;

            case AbilitySlotType.Escape:
                if (escapeAbility != null)
                {
                    Debug.Log("Ескейп");
                    if (CanUseAbility(escapeAbility))
                        escapeAbility.Activate(gameObject);
                }
                break;
        }
    }

    private bool CanUseAbility(ActiveAbility ability)
    {
        return resourceSystem.Consume(ability.resourceCost);
    }

    // Метод для назначения способности в слот
    public bool AssignAbilityToSlot(Ability ability, AbilitySlotType slotType)
    {
        switch (slotType)
        {
            case AbilitySlotType.Major:
            case AbilitySlotType.Minor:
            case AbilitySlotType.Escape:
                if (ability is ActiveAbility activeAbility)
                {
                    AssignActiveAbilityToSlot(activeAbility, slotType);
                    return true;
                }
                break;

            case AbilitySlotType.Passive:
                if (ability is PassiveAbility passiveAbility)
                {
                    passiveAbility.ApplyEffect(gameObject);
                    this.passiveAbility = passiveAbility;
                    return true;
                }
                break;
        }
        return false;
    }

    // Вспомогательный метод для назначения активных способностей в слоты
    private void AssignActiveAbilityToSlot(ActiveAbility activeAbility, AbilitySlotType slotType)
    {
        switch (slotType)
        {
            case AbilitySlotType.Major:
                majorAbility = activeAbility;
                break;
            case AbilitySlotType.Minor:
                minorAbility = activeAbility;
                break;
            case AbilitySlotType.Escape:
                escapeAbility = activeAbility;
                break;
        }
    }
}
