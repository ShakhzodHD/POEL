using UnityEngine.EventSystems;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private BaseInventoryItem item;
    private GridInventoryUI inventoryUI;

    private Vector2 originalPosition;
    private Vector2Int startDragGridPosition;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(BaseInventoryItem item, GridInventoryUI ui)
    {
        this.item = item;
        inventoryUI = ui;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        float width = item.width * inventoryUI.cellSize + (item.width - 1) * inventoryUI.spacing;
        float height = item.height * inventoryUI.cellSize + (item.height - 1) * inventoryUI.spacing;
        rectTransform.sizeDelta = new Vector2(width, height);

        if (item.position.x >= 0)
        {
            rectTransform.anchoredPosition = inventoryUI.GetCellPosition(item.position.x, item.position.y);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragGridPosition = item.position;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        inventoryUI.inventory.RemoveItem(item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryUI.itemsContainer,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );
        rectTransform.anchoredPosition = localPoint;

        Vector2Int gridPos = inventoryUI.GetGridPosition(localPoint);
        bool canPlace = inventoryUI.inventory.CanPlaceItem(item, gridPos.x, gridPos.y);
        inventoryUI.HighlightSlots(item, gridPos, canPlace);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

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
            inventoryUI.inventory.PlaceItem(item, gridPos.x, gridPos.y);
            UpdateVisuals();
        }
        else
        {
            inventoryUI.inventory.PlaceItem(item, startDragGridPosition.x, startDragGridPosition.y);
            rectTransform.anchoredPosition = originalPosition;
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
}
