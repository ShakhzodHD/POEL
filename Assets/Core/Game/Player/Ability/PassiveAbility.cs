using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Ability", menuName = "Abilities/Passive Ability")]
public class PassiveAbility : Ability
{
    public int effectValue; // Величина эффекта, например, увеличение урона или брони
    public float duration; // Продолжительность эффекта для временных аур

    public void ApplyEffect(GameObject user)
    {
        // Логика применения эффекта (например, добавление бонусов к статам)
        Debug.Log("Применен эффект ");
    }

    public void RemoveEffect(GameObject user)
    {
        // Логика отмены эффекта, если нужно (например, при завершении действия ауры)
    }
}
