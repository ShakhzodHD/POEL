using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public CharacterClass characterClass;
    public ActiveAbility abilityMajor;
    public ActiveAbility abilityMinor;
    public ActiveAbility abilityEscape;
    public PassiveAbility abilityPassive;
    public List<Character> characters = new();
    public int idSelectedCharacter;
    private void Awake()
    {
        abilityMajor = new ActiveAbility();
        abilityMinor = new ActiveAbility();
        abilityEscape = new ActiveAbility();
        abilityPassive = new PassiveAbility();
    }
    public void AddCharacter(Character character)
    {
        characters.Add(character);
    }
}
