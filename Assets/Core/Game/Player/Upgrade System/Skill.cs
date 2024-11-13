using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int levelRequired; // ������� ��������� ��� �������������
    public int cost; // ��������� ������������� (��������, ���� �������)
    public Sprite icon; // ������ ��� �����������
    public Skill[] prerequisites; // ������ �������, ������� ������ ���� �������������� �� �����
    public void ApplyEffect(Character currentCharacter)
    {
        foreach (Effect effect in effects)
        {
            if (effect.modifierValue > 0)
            {
                currentCharacter.Stats.IncreaseStat(effect.statType, effect.modifierValue);
                Debug.Log($"{skillName} applied: {effect.statType} increased by {effect.modifierValue}");
            }
            else if (effect.modifierValue < 0)
            {
                currentCharacter.Stats.DecreaseStat(effect.statType, effect.modifierValue);
                Debug.Log($"{skillName} applied: {effect.statType} decreased by {effect.modifierValue}");
            }
            else
            {
                return;
            }
        }
    }
    [System.Serializable]
    public class Effect
    {
        public Stat statType;
        public float modifierValue;
    }
    public Effect[] effects;
}
