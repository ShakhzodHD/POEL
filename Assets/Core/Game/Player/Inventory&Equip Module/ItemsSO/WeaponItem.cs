using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
public class WeaponItem : Item
{
    public ItemRarity Rarity;
    public override void Use()
    {
        Debug.Log($"You equip {ItemName}, rarity {Rarity}.");
    }
}
