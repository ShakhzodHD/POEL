using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Ability", menuName = "Abilities/Passive Ability")]
public class PassiveAbility : Ability
{
    public int effectValue; // �������� �������, ��������, ���������� ����� ��� �����
    public float duration; // ����������������� ������� ��� ��������� ���

    public void ApplyEffect(GameObject user)
    {
        // ������ ���������� ������� (��������, ���������� ������� � ������)
        Debug.Log("�������� ������ ");
    }

    public void RemoveEffect(GameObject user)
    {
        // ������ ������ �������, ���� ����� (��������, ��� ���������� �������� ����)
    }
}
