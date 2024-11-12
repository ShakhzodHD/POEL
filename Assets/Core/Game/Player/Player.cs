using UnityEngine;

public class Player : MonoBehaviour
{
    public int idCharacter {  get; private set; }

    [SerializeField] private ActiveAbility majorAbility;
    [SerializeField] private ActiveAbility minorAbility;
    [SerializeField] private ActiveAbility escapeAbility;
    [SerializeField] private PassiveAbility passiveAbility;

    private Character currentCharacter;
    private CharacterClass characterClass;
    private PlayerMovement playerMovement;

    private HealthSystem healthSystem;
    private ResourceSystem resourceSystem;
    private Stats stats;
    private void Start()
    {
        Boostrap.Instance.TopDownCamera.SetTarget(gameObject.transform);
        Boostrap.Instance.InitPlayer(this);

        playerMovement = GetComponent<PlayerMovement>();

        InitCharacter();
    }
    private void Update() // Временно
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Upgrade);
        }
    }
    private void InitCharacter()
    {
        int selectedIndex = Boostrap.Instance.PlayerData.idSelectedCharacter;
        var characters = Boostrap.Instance.PlayerData.characters;

        if (selectedIndex >= 0 && selectedIndex < characters.Count)
        {
            var character = characters[selectedIndex];
            characterClass = character.CharacterClass;

            healthSystem = new HealthSystem(characterClass.baseHealth);
            resourceSystem = new ResourceSystem(characterClass.baseResource);

            playerMovement.MovementSpeed = character.Speed;

            stats = character.Stats;

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
    public bool UnlockSkill(Skill skill)
    {
        if (currentCharacter.UnlockSkill(skill))
        {
            ApplySkillEffects(skill);
            return true;
        }
        return false;
    }
    public void ApplySkillEffects(Skill skill)
    {
        Debug.Log("Apply Skill Effec: " + skill.name);
        //if (skill is StatModifierSkill statSkill)
        //{
        //    stats.ModifyStat(statSkill.statType, statSkill.modifierValue);
        //    UpdateSystemsFromStats();
        //}
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
