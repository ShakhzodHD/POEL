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
}
