using System.Collections.Generic;

public class Equipment
{
    private Dictionary<ItemType, EquipmentSlot> equipmentSlots;

    public Equipment()
    {
        equipmentSlots = new Dictionary<ItemType, EquipmentSlot>
        {
            { ItemType.Weapon, new EquipmentSlot { AllowedType = ItemType.Weapon } },
            { ItemType.Armor, new EquipmentSlot { AllowedType = ItemType.Armor } },
            { ItemType.Helmet, new EquipmentSlot { AllowedType = ItemType.Helmet } },
        };
    }

    public bool EquipItem(Item item)
    {
        if (equipmentSlots.ContainsKey(item.Type))
        {
            return equipmentSlots[item.Type].Equip(item);
        }
        return false;
    }

    public void UnequipItem(ItemType itemType)
    {
        if (equipmentSlots.ContainsKey(itemType))
        {
            equipmentSlots[itemType].Unequip();
        }
    }

    public Item GetEquippedItem(ItemType itemType)
    {
        return equipmentSlots.ContainsKey(itemType) ? equipmentSlots[itemType].EquippedItem : null;
    }
}
