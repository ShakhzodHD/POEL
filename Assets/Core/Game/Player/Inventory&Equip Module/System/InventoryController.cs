using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryRenderer))]
public class InventoryController : MonoBehaviour, IInventoryController, IPointerDownHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    public InventoryRenderer inventoryRenderer;

    private Canvas canvas;
    public IInventoryItem itemToDrag;
    public PointerEventData currentEventData;
    private IInventoryItem lastHoveredItem;
    private static InventoryDraggedItem draggedItem;

    public Action<IInventoryItem> OnItemHovered { get; set; }
    public Action<IInventoryItem> OnItemPickedUp { get; set; }
    public Action<IInventoryItem> OnItemAdded { get; set; }
    public Action<IInventoryItem> OnItemSwapped { get; set; }
    public Action<IInventoryItem> OnItemReturned { get; set; }
    public Action<IInventoryItem> OnItemDropped { get; set; }
    public InventoryManager Inventory => (InventoryManager)inventoryRenderer.inventory;
    private void Awake()
    {
        inventoryRenderer = GetComponent<InventoryRenderer>();
        if (inventoryRenderer == null) { throw new NullReferenceException("Could not find a renderer. This is not allowed!"); }

        var canvases = GetComponentsInParent<Canvas>();
        if (canvases.Length == 0) { throw new NullReferenceException("Could not find a canvas."); }
        canvas = canvases[canvases.Length - 1];
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (draggedItem != null) return;
        var grid = ScreenToGrid(eventData.position);
        itemToDrag = Inventory.GetAtPoint(grid);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryRenderer.ClearSelection();

        if (itemToDrag == null || draggedItem != null) return;
        var localPosition = ScreenToLocalPositionInRenderer(eventData.position);
        var itemOffest = inventoryRenderer.GetItemOffset(itemToDrag);
        var offset = itemOffest - localPosition;

        draggedItem = new InventoryDraggedItem(
            this,
            itemToDrag.Position,
            itemToDrag,
            offset,
            canvas
        );

        Inventory.TryRemove(itemToDrag);

        OnItemPickedUp?.Invoke(itemToDrag);
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentEventData = eventData;
        if (draggedItem != null)
        {
            //draggedItem.Position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem == null) return;

        var mode = draggedItem.Drop(eventData.position);

        switch (mode)
        {
            case DropMode.Added:
                OnItemPickedUp?.Invoke(itemToDrag);
                break;
            case DropMode.Swapped:
                OnItemPickedUp?.Invoke(itemToDrag);
                break;
            case DropMode.Returned:
                OnItemPickedUp?.Invoke(itemToDrag);
                break;
            case DropMode.Dropped:
                OnItemPickedUp?.Invoke(itemToDrag);
                ClearHoveredItem();
                break;
        }

        draggedItem = null;
        currentEventData = null;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.currentController = null;
            inventoryRenderer.ClearSelection();
            Boostrap.Instance.CentralizedClickHandler.IsDropItem = false;
        }
        else { ClearHoveredItem(); }
        currentEventData = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.currentController = this;
            Boostrap.Instance.CentralizedClickHandler.IsDropItem = true;
        }
        currentEventData = eventData;
    }
    private void Update()
    {
        if (currentEventData == null) return;

        if (draggedItem == null)
        {
            var grid = ScreenToGrid(currentEventData.position);
            var item = Inventory.GetAtPoint(grid);
            if (item == lastHoveredItem) return;
            OnItemHovered?.Invoke(item);
            lastHoveredItem = item;
        }
        else
        {
            draggedItem.Position = currentEventData.position;
        }
    }
    private void ClearHoveredItem()
    {
        if (lastHoveredItem != null)
        {
            OnItemHovered?.Invoke(null);
        }
        lastHoveredItem = null;
    }
    public Vector2Int ScreenToGrid(Vector2 screenPoint)
    {
        var pos = ScreenToLocalPositionInRenderer(screenPoint);
        var sizeDelta = inventoryRenderer.RectTransform.sizeDelta;
        pos.x += sizeDelta.x / 2;
        pos.y += sizeDelta.y / 2;
        return new Vector2Int(Mathf.FloorToInt(pos.x / inventoryRenderer.CellSize.x), Mathf.FloorToInt(pos.y / inventoryRenderer.CellSize.y));
    }
    private Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryRenderer.RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out var localPosition
        );
        return localPosition;
    }
}
