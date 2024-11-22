using System.Collections.Generic;
using UnityEngine;

public class GridEquip : MonoBehaviour
{
    private Dictionary<TypeSlotEnum, EquipmentSlot> equipmentSlots = new();
    private GridInventory inventory;

    [SerializeField] private EquipmentSlot weaponSlot;
    [SerializeField] private EquipmentSlot armorSlot;
    [SerializeField] private EquipmentSlot glovesSlot;
    public void Initialize(GridInventory inv)
    {
        inventory = inv;

        equipmentSlots[TypeSlotEnum.Weapon] = weaponSlot;
        equipmentSlots[TypeSlotEnum.Armor] = armorSlot;
        equipmentSlots[TypeSlotEnum.Gloves] = glovesSlot;

        foreach (var slot in equipmentSlots.Values)
        {
            slot.Initialize(this);
        }
    }
    public bool IsEquipped(BaseInventoryItem item)
    {
        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            return slot.GetEquippedItem() == item;
        }
        return false;
    }
    public void EquipItem(BaseInventoryItem item)
    {
        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            var currentItem = slot.GetEquippedItem();
            if (currentItem != null)
            {
                UnequipItem(currentItem);
            }

            slot.EquipItem(item);
            Debug.Log($"Equipped item of type {item.typeItem}");
        }
    }
    public void UnequipItem(BaseInventoryItem item)
    {
        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            if (slot.GetEquippedItem() == item)
            {
                slot.UnequipItem();
                if (!inventory.TryPlaceItem(item))
                {
                    Debug.LogWarning("No space in inventory for unequipped item!");
                    // Здесь можно добавить логику для случая, когда нет места в инвентаре
                }
            }
        }
    }
    public BaseInventoryItem GetItemFromUI(InventoryItemUI itemUI)
    {
        return itemUI.GetComponent<InventoryItemUI>().GetItem();
    }
}
