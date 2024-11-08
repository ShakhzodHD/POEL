using UnityEngine;

[CreateAssetMenu(fileName = "New Active Ability", menuName = "Abilities/Active Ability")]
public class ActiveAbility : Ability
{
    public int resourceCost;

    public virtual void Activate(GameObject user)
    {
        // Логика проверки ресурсов и активации способности
    }
}
