using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Character/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    public string className;  // �������� ������
    public Sprite classIcon;  // ������ ��� UI
    public string description;  // �������� ������

    public ActiveAbility[] startingMajorAbilities;
    public ActiveAbility[] startingMinorAbilities;
    public ActiveAbility[] startingEscapeAbilities;
    public PassiveAbility[] startingPassiveAbilities;

    // ������� �����
    public Stat healthStat;
    public ResourceType resourceType;
    public Stat resourceValue;

    // �������������� �����

    // ��������� ��� �������� �������� ������
    [System.Serializable]
    public class Stat
    {
        public int baseValue;  // ������� ��������
        public int currentValue;  // ������� �������� (���� ����������� ������������ ���������)
    }
}
