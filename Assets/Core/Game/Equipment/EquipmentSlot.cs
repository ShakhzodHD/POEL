using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public TypeSlotEnum slotType;

    [SerializeField] private Image slotIcon;
    [SerializeField] private Image itemIcon;

    [SerializeField] private GridEquip gridEquip;

    private BaseInventoryItem equippedItem;

    private void Awake()
    {
        if (itemIcon == null)
        {
            itemIcon = transform.Find("ItemIcon")?.GetComponent<Image>();
        }
        if (slotIcon == null)
        {
            slotIcon = GetComponent<Image>();
        }

        if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }
    }
    public void Initialize(GridEquip grid)
    {
        //gridEquip = grid;

        //Debug.Log("State Grid Equip" + gridEquip);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var itemUI = eventData.pointerDrag.GetComponent<InventoryItemUI>();
        if (itemUI != null)
        {
            var newItem = itemUI.GetItem();
            if (newItem != null && newItem.typeItem == slotType)
            {
                if (equippedItem == newItem)
                {
                    return;
                }

                if (equippedItem != null)
                {
                    var currentEquippedItem = equippedItem;
                    UnequipItem();
                    gridEquip.UnequipItem(currentEquippedItem);
                }

                EquipItem(newItem);
                itemUI.isEquipped = true;

                itemUI.transform.SetParent(transform);
                RectTransform itemRect = itemUI.GetComponent<RectTransform>();
                if (itemRect != null)
                {
                    itemRect.anchoredPosition = Vector2.zero;
                    itemRect.anchorMin = new Vector2(0.5f, 0.5f);
                    itemRect.anchorMax = new Vector2(0.5f, 0.5f);
                    itemRect.pivot = new Vector2(0.5f, 0.5f);
                }
            }
        }
    }

    public void EquipItem(BaseInventoryItem item)
    {
        if (equippedItem != null)
        {
            Debug.LogWarning("Attempting to equip item when slot is not empty!");
            return;
        }

        equippedItem = item;

        if (itemIcon != null)
        {
            var inventoryItem = item as InventoryItem;
            if (inventoryItem != null && inventoryItem.icon != null)
            {
                itemIcon.sprite = inventoryItem.icon;
                itemIcon.enabled = true;
            }
        }
    }

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            var itemUI = GetEquippedItemUI();
            if (itemUI != null)
            {
                itemUI.isEquipped = false;

                itemUI.transform.SetParent(itemUI.GetInventoryContainer());

                RectTransform itemRect = itemUI.GetComponent<RectTransform>();
                if (itemRect != null)
                {
                    itemRect.anchoredPosition = Vector2.zero;
                    itemRect.anchorMin = new Vector2(0f, 1f);
                    itemRect.anchorMax = new Vector2(0f, 1f);
                    itemRect.pivot = new Vector2(0f, 1f);
                }

                itemUI.UpdateVisuals();
            }

            equippedItem = null;

            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
        }
    }
    private InventoryItemUI GetEquippedItemUI()
    {
        return GetComponentInChildren<InventoryItemUI>();
    }

    public BaseInventoryItem GetEquippedItem()
    {
        return equippedItem;
    }

    public void OnItemTakenFromSlot()
    {
        if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }
    }

    public void OnItemReturnedToSlot()
    {
        if (itemIcon != null && equippedItem != null)
        {
            var inventoryItem = equippedItem as InventoryItem;
            if (inventoryItem != null && inventoryItem.icon != null)
            {
                itemIcon.sprite = inventoryItem.icon;
                itemIcon.enabled = true;
            }
        }
    }
}