public class Character
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public CharacterClass CharacterClass { get; private set; }
    public ActiveAbility MajorAbility { get; private set; }
    public ActiveAbility MinorAbility { get; private set; }
    public ActiveAbility EscapeAbility { get; private set; }
    public PassiveAbility PassiveAbility { get; private set; }

    public Character(int id, string name, CharacterClass characterClass, ActiveAbility major, ActiveAbility minor, ActiveAbility escape, PassiveAbility passive)
    {
        Name = name;
        CharacterClass = characterClass;
        MajorAbility = major;
        MinorAbility = minor;
        EscapeAbility = escape;
        PassiveAbility = passive;
        Id = id;
    }
}
