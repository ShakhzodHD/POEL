using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Character/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    public string className;  // Название класса
    public Sprite classIcon;  // Иконка для UI
    public string description;  // Описание класса

    public ActiveAbility[] startingMajorAbilities;
    public ActiveAbility[] startingMinorAbilities;
    public ActiveAbility[] startingEscapeAbilities;
    public PassiveAbility[] startingPassiveAbilities;

    // Базовые статы
    public Stat healthStat;
    public ResourceType resourceType;
    public Stat resourceValue;

    // Дополнительные статы

    // Структура для хранения значений статов
    [System.Serializable]
    public class Stat
    {
        public int baseValue;  // Базовое значение
        public int currentValue;  // Текущее значение (если планируется динамическое изменение)
    }
}
