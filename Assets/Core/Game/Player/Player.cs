using UnityEngine;

public class Player : MonoBehaviour
{
    public int idCharacter {  get; private set; }

    [SerializeField] private ActiveAbility majorAbility;
    [SerializeField] private ActiveAbility minorAbility;
    [SerializeField] private ActiveAbility escapeAbility;
    [SerializeField] private PassiveAbility passiveAbility;

    private CharacterClass characterClass;

    private void Start()
    {
        Boostrap.Instance.TopDownCamera.SetTarget(gameObject.transform);

        int selectedIndex = Boostrap.Instance.PlayerData.idSelectedCharacter;
        var characters = Boostrap.Instance.PlayerData.characters;

        if (selectedIndex >= 0 && selectedIndex < characters.Count)
        {
            var character = characters[selectedIndex];
            characterClass = character.CharacterClass;
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
    //public void Initialize(CharacterClass characterClass)
    //{
    //    // Инициализация базовых статов
    //    //health = characterClass.healthStat.baseValue;
    //    //mana = characterClass.manaStat.baseValue;

    //    // Установка начальных способностей
    //    //AssignAbilityToSlot(characterClass.startingAbilities[0], AbilitySlotType.Major);
    //    //AssignAbilityToSlot(characterClass.startingAbilities[1], AbilitySlotType.Minor);
    //    //AssignAbilityToSlot(characterClass.startingAbilities[2], AbilitySlotType.Escape);
    //}

    public void UseAbility(AbilitySlotType slotType)
    {
        switch (slotType)
        {
            case AbilitySlotType.Major:
                if (majorAbility != null)
                {
                    Debug.Log("Мажор");
                    // Проверяем ресурс и активируем способность
                    if (CanUseAbility(majorAbility))
                        majorAbility.Activate(gameObject);
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

    // Метод для проверки ресурса перед использованием способности
    private bool CanUseAbility(ActiveAbility ability)
    {
        // Здесь должна быть логика проверки ресурса, например:
        // - достаточно ли маны или выносливости у игрока для использования способности
        // Временно возвращаем true для примера
        return true;
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
