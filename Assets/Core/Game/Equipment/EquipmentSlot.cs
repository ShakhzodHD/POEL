using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public TypeSlotEnum slotType;
    [SerializeField] private Image icon;

    private BaseInventoryItem equippedItem;
    private GridEquip gridEquip;
    public void Initialize(GridEquip grid)
    {
        gridEquip = grid;
    }
    public void OnDrop(PointerEventData eventData)
    {
        var itemUI = eventData.pointerDrag.GetComponent<InventoryItemUI>();
        if (itemUI != null)
        {
            var item = gridEquip.GetItemFromUI(itemUI);
            if (item.typeItem == slotType)
            {
                gridEquip.EquipItem(item);
            }
        }
    }
    public void EquipItem(BaseInventoryItem item)
    {
        equippedItem = item;
        if (icon != null)
        {
            icon.sprite = (item as InventoryItem)?.icon;
            icon.enabled = true;
        }
    }
    public void UnequipItem()
    {
        equippedItem = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public BaseInventoryItem GetEquippedItem()
    {
        return equippedItem;
    }
}
