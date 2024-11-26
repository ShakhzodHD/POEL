using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private ActiveAbility majorAbility;
    [SerializeField] private ActiveAbility minorAbility;
    [SerializeField] private ActiveAbility escapeAbility;
    [SerializeField] private PassiveAbility passiveAbility;

    private Character currentCharacter;
    private CharacterClass characterClass;
    private PlayerInput input;
    private PlayerMovement playerMovement;

    private HealthSystem healthSystem;
    private ResourceSystem resourceSystem;
    private void Start()
    {
        Boostrap.Instance.TopDownCamera.SetTarget(gameObject.transform);

        input = GetComponent<PlayerInput>();
        Boostrap.Instance.PlayerData.input = input;

        playerMovement = GetComponent<PlayerMovement>();

        InitCharacter();
        Boostrap.Instance.UIManager.InitUpgradePanel(this);
    }

    private void OnLevelUp(int newLevel)
    {
        if (newLevel <= 0)
        {
            throw new ArgumentException("New level cannot be negative: " + newLevel);
        }

        currentCharacter.AddSkillPoints(1);
        currentCharacter.Level = newLevel;
        

        Debug.Log($"Player.cs LevelUP! New Level: {newLevel}");
    }

    private void OnExperienceGained(int currentExperience, int experienceToNextLevel)
    {
        Debug.Log($"OnExperienceGained: CurrentExp {currentCharacter} CurrentExpToNextLevel {experienceToNextLevel}");
        currentCharacter.CurrentExperience = currentExperience;
        currentCharacter.ExperienceToNextLevel = experienceToNextLevel;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T)) 
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
        currentCharacter = Boostrap.Instance.PlayerData.selectedCharacter; ;
        characterClass = currentCharacter.CharacterClass;

        healthSystem = new HealthSystem(characterClass.baseHealth);
        resourceSystem = new ResourceSystem(characterClass.baseResource);

        playerMovement.MovementSpeed = currentCharacter.Speed;

        Boostrap.Instance.ExperienceSystem.Initialize();
        Boostrap.Instance.ExperienceSystem.OnExperienceGained += OnExperienceGained;
        Boostrap.Instance.ExperienceSystem.OnLevelUp += OnLevelUp;

        foreach (var stat in currentCharacter.Stats.stats)
        {
            Debug.Log($"{stat.Key}: {stat.Value}");
        }

        majorAbility = currentCharacter.MajorAbility;
        minorAbility = currentCharacter.MinorAbility;
        escapeAbility = currentCharacter.EscapeAbility;
        passiveAbility = currentCharacter.PassiveAbility;

        passiveAbility.ApplyEffect(gameObject);
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
                    Debug.Log("Major");
                    if (CanUseAbility(majorAbility))
                        majorAbility.Activate(gameObject);
                    else { Debug.Log("Not enouth resourse"); }
                }
                break;

            case AbilitySlotType.Minor:
                if (minorAbility != null)
                {
                    Debug.Log("Minor");
                    if (CanUseAbility(minorAbility))
                        minorAbility.Activate(gameObject);
                }
                break;

            case AbilitySlotType.Escape:
                if (escapeAbility != null)
                {
                    Debug.Log("Escape");
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
