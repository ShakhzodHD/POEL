using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Character/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    public string className;
    public Sprite classIcon;
    public string description;

    public ActiveAbility[] startingMajorAbilities;
    public ActiveAbility[] startingMinorAbilities;
    public ActiveAbility[] startingEscapeAbilities;
    public PassiveAbility[] startingPassiveAbilities;

    public float baseHealth = 100f;
    public float baseResource = 50f;
    public float baseSpeed = 5.0f;

    public SerializableDictionaryBase<Stat, float> baseStatsAttribute;

    public Stats BaseStats
    {
        get { return new Stats(new Dictionary<Stat, float>(baseStatsAttribute)); }
    }
}
