using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int levelRequired; // ”ровень персонажа дл€ разблокировки
    public int cost; // —тоимость разблокировки (например, очки навыков)
    public Sprite icon; // »конка дл€ отображени€
    public Skill[] prerequisites; // —писок навыков, которые должны быть разблокированы до этого
}
