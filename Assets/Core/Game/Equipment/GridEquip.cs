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
        // Проверяем, что предмет не экипирован
        if (IsEquipped(item))
        {
            Debug.Log($"Предмет {item} уже экипирован.");
            return;
        }

        if (equipmentSlots.TryGetValue(item.typeItem, out var slot))
        {
            var currentItem = slot.GetEquippedItem();
            if (currentItem != null)
            {
                // Сначала снимаем текущий предмет
                UnequipItem(currentItem);
            }

            // Проверяем, что слот действительно пустой
            if (slot.GetEquippedItem() == null)
            {
                slot.EquipItem(item);
                Debug.Log($"Предмет успешно экипирован в слот {item.typeItem}");
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
                // Сначала проверяем, можно ли положить предмет в инвентарь
                if (inventory.TryPlaceItem(item))
                {
                    slot.UnequipItem();
                    Debug.Log($"Предмет успешно снят и помещен в инвентарь");
                }
                else
                {
                    Debug.LogWarning("Нет места в инвентаре для снятого предмета!");
                    return;
                    // Можно добавить дополнительную логику здесь
                }
            }
        }
    }
    public BaseInventoryItem GetItemFromUI(InventoryItemUI itemUI)
    {
        return itemUI.GetComponent<InventoryItemUI>().GetItem();
    }
}
