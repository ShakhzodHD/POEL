using System.Collections.Generic;

public class Character
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public float Speed { get; private set; }
    public CharacterClass CharacterClass { get; private set; }
    public ActiveAbility MajorAbility { get; private set; }
    public ActiveAbility MinorAbility { get; private set; }
    public ActiveAbility EscapeAbility { get; private set; }
    public PassiveAbility PassiveAbility { get; private set; }
    public HealthSystem Health { get; private set; }
    public ResourceSystem Resource { get; private set; }
    public Stats Stats { get; set; }
    public List<Skill> UnlockedSkills { get; private set; }
    public int SkillPoints { get; private set; }
    public int Level { get; set; }
    public int CurrentExperience { get; set; }
    public int ExperienceToNextLevel { get; set; }
    public InventoryProvider InventoryProvider { get; private set; }
    public InventoryManager InventoryManager { get; private set; }

    public Character(int id, string name, 
        CharacterClass characterClass, ActiveAbility major, ActiveAbility minor, ActiveAbility escape, PassiveAbility passive,
        float maxHealth, float maxResource, float movementSpeed, Stats stats, SizeInventory mainInventory)
    {
        Name = name;
        CharacterClass = characterClass;
        MajorAbility = major;
        MinorAbility = minor;
        EscapeAbility = escape;
        PassiveAbility = passive;
        Id = id;
        Health = new HealthSystem(maxHealth);
        Resource = new ResourceSystem(maxResource);
        Speed = movementSpeed;
        Stats = stats.Clone();
        UnlockedSkills = new List<Skill>();
        SkillPoints = 0;
        Level = 0;
        CurrentExperience = 0;
        ExperienceToNextLevel = 0;
        InventoryProvider = new(mainInventory.renderMode, mainInventory.maximumAlowedItemCount, mainInventory.allowedItem);
        InventoryManager = new InventoryManager(InventoryProvider, mainInventory.width, mainInventory.height);
    }
    public void AddSkillPoints(int points)
    {
        SkillPoints += points;
    }
    public bool UnlockSkill(Skill skill)
    {
        if (CanUnlockSkill(skill))
        {
            UnlockedSkills.Add(skill);
            SkillPoints -= skill.cost;
            return true;
        }
        return false;
    }
    public bool CanUnlockSkill(Skill skill)
    {
        if (skill == null) return true;
        if (UnlockedSkills.Contains(skill)) return false;
        foreach (Skill prerequisite in skill.prerequisites)
        {
            if (!UnlockedSkills.Contains(prerequisite)) return false;
        }
        return SkillPoints >= skill.cost;
    }
}
