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
            Debug.LogError("�������� ������ ���������.");
        }
    }
    //public void Initialize(CharacterClass characterClass)
    //{
    //    // ������������� ������� ������
    //    //health = characterClass.healthStat.baseValue;
    //    //mana = characterClass.manaStat.baseValue;

    //    // ��������� ��������� ������������
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
                    Debug.Log("�����");
                    // ��������� ������ � ���������� �����������
                    if (CanUseAbility(majorAbility))
                        majorAbility.Activate(gameObject);
                }
                break;

            case AbilitySlotType.Minor:
                if (minorAbility != null)
                {
                    Debug.Log("�����");
                    if (CanUseAbility(minorAbility))
                        minorAbility.Activate(gameObject);
                }
                break;

            case AbilitySlotType.Escape:
                if (escapeAbility != null)
                {
                    Debug.Log("������");
                    if (CanUseAbility(escapeAbility))
                        escapeAbility.Activate(gameObject);
                }
                break;
        }
    }

    // ����� ��� �������� ������� ����� �������������� �����������
    private bool CanUseAbility(ActiveAbility ability)
    {
        // ����� ������ ���� ������ �������� �������, ��������:
        // - ���������� �� ���� ��� ������������ � ������ ��� ������������� �����������
        // �������� ���������� true ��� �������
        return true;
    }

    // ����� ��� ���������� ����������� � ����
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

    // ��������������� ����� ��� ���������� �������� ������������ � �����
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
