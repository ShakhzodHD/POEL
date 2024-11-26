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
        if (IsEquipped(item))
        {
            return;
        }

        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            var currentItem = slot.GetEquippedItem();
            if (currentItem != null)
            {
                UnequipItem(currentItem);
            }

            if (slot.GetEquippedItem() == null)
            {
                slot.EquipItem(item);
            }
            else
            {
                Debug.LogError("Failed to unequip current item!");
            }
        }
    }
    public void UnequipItem(BaseInventoryItem item)
    {
        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            if (slot.GetEquippedItem() == item)
            {
                if (inventory.TryPlaceItem(item))
                {
                    slot.UnequipItem();
                }
                else
                {
                    return;
                }
            }
        }
    }
    public BaseInventoryItem GetItemFromUI(InventoryItemUI itemUI)
    {
        return itemUI.GetComponent<InventoryItemUI>().GetItem();
    }
}
