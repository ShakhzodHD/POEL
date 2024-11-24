using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool isEquipped;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private BaseInventoryItem item;
    private GridInventoryUI inventoryUI;
    private GridEquip gridEquip;
    private EquipmentSlot currentEquipSlot;
    private InventoryEquipmentManager manager;

    private Vector2 originalPosition;
    private Vector2Int startDragGridPosition;
    private Transform originalParent;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(BaseInventoryItem item, GridInventoryUI ui, GridEquip equipSystem, InventoryEquipmentManager manager)
    {
        this.item = item;
        inventoryUI = ui;
        gridEquip = equipSystem;
        this.manager = manager;
        UpdateVisuals();
    }

    public void SetEquipSlot(EquipmentSlot slot)
    {
        currentEquipSlot = slot;
        if (slot != null)
        {
            isEquipped = true;
            transform.SetParent(slot.transform);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            isEquipped = false;
            transform.SetParent(inventoryUI.itemsContainer);
            UpdateVisuals();
        }
    }

    public void UpdateVisuals()
    {
        float width = item.width * inventoryUI.cellSize + (item.width - 1) * inventoryUI.spacing;
        float height = item.height * inventoryUI.cellSize + (item.height - 1) * inventoryUI.spacing;
        rectTransform.sizeDelta = new Vector2(width, height);

        if (!isEquipped)
        {
            // Убедимся, что предмет находится в контейнере инвентаря
            if (transform.parent != GetInventoryContainer())
            {
                transform.SetParent(GetInventoryContainer());
            }
            if (item.position.x >= 0)
            {
                rectTransform.anchoredPosition = inventoryUI.GetCellPosition(item.position.x, item.position.y);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEquipped)
        {
            // Проверяем, есть ли место в инвентаре перед началом перетаскивания
            if (!inventoryUI.inventory.CanFindSpaceForItem(item))
            {
                Debug.LogWarning("Нет места в инвентаре для снятия предмета!");
                return; // Отменяем начало перетаскивания
            }
        }

        originalParent = transform.parent;
        startDragGridPosition = item.position;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        if (isEquipped)
        {
            currentEquipSlot.OnItemTakenFromSlot();
        }
        else
        {
            inventoryUI.inventory.RemoveItem(item);
        }

        transform.SetParent(inventoryUI.dragContainer);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)inventoryUI.dragContainer,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }

        if (!isEquipped)
        {
            // Проверяем возможность размещения в инвентаре только если предмет не экипирован
            Vector2Int gridPos = inventoryUI.GetGridPosition(localPoint);
            bool canPlace = inventoryUI.inventory.CanPlaceItem(item, gridPos.x, gridPos.y);
            inventoryUI.HighlightSlots(item, gridPos, canPlace);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Проверяем, находится ли курсор над слотом экипировки
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool placed = false;

        foreach (var result in results)
        {
            EquipmentSlot equipSlot = result.gameObject.GetComponent<EquipmentSlot>();
            if (equipSlot != null && equipSlot.slotType == item.typeItem)
            {
                // Экипируем предмет
                gridEquip.EquipItem(item);
                SetEquipSlot(equipSlot);
                placed = true;
                break;
            }
        }

        if (!placed)
        {
            // Пытаемся разместить в инвентаре
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                inventoryUI.itemsContainer,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint
            );

            Vector2Int gridPos = inventoryUI.GetGridPosition(localPoint);

            if (inventoryUI.inventory.CanPlaceItem(item, gridPos.x, gridPos.y))
            {
                transform.SetParent(inventoryUI.itemsContainer);
                inventoryUI.inventory.PlaceItem(item, gridPos.x, gridPos.y);
                SetEquipSlot(null);
                UpdateVisuals();
            }
            else
            {
                // Возвращаем предмет на исходную позицию
                transform.SetParent(originalParent);
                if (isEquipped)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                    currentEquipSlot.OnItemReturnedToSlot();
                }
                else
                {
                    inventoryUI.inventory.PlaceItem(item, startDragGridPosition.x, startDragGridPosition.y);
                    rectTransform.anchoredPosition = originalPosition;
                }
            }
        }

        inventoryUI.ResetAllHighlights();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Delete");
            //Boostrap.Instance.PlayerData.selectedCharacter.RemoveItemFromInventory(item);
        }
    }

    public BaseInventoryItem GetItem()
    {
        return item;
    }
    public Transform GetInventoryContainer()
    {
        return inventoryUI.itemsContainer;
    }
}
