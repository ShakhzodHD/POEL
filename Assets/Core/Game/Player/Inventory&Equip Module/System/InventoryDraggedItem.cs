using UnityEngine;
using UnityEngine.UI;

public class InventoryDraggedItem
{
    public InventoryController currentController;

    private readonly Canvas canvas;
    private readonly RectTransform canvasRect;
    private readonly Image image;
    private Vector2 offset;

    public InventoryController OriginalController { get; private set; }
    public Vector2Int OriginPoint { get; private set; }
    public IInventoryItem Item { get; private set; }
    public Vector2 Position
    {
        set
        {
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, value + offset, camera, out var newValue);
            image.rectTransform.localPosition = newValue;


            if (currentController != null)
            {
                Item.Position = currentController.ScreenToGrid(value + offset + GetDraggedItemOffset(currentController.inventoryRenderer, Item));
                var canAdd = currentController.Inventory.CanAddAt(Item, Item.Position) || CanSwap();
                currentController.inventoryRenderer.SelectItem(Item, !canAdd, Color.white);
            }

            offset = Vector2.Lerp(offset, Vector2.zero, Time.deltaTime * 10f);
        }
    }
    public InventoryDraggedItem(InventoryController originalController, Vector2Int originPoint, IInventoryItem item, Vector3 offset, Canvas canvas)
    {
        OriginalController = originalController;
        currentController = OriginalController;
        OriginPoint = originPoint;
        Item = item;

        this.canvas = canvas;
        canvasRect = canvas.transform as RectTransform;

        this.offset = offset;

        image = new GameObject("DraggedItem").AddComponent<Image>();
        image.raycastTarget = false;
        image.transform.SetParent(canvas.transform);
        image.transform.SetAsLastSibling();
        image.transform.localScale = Vector3.one;
        image.sprite = item.Sprite;
        image.SetNativeSize();
    }
    public DropMode Drop(Vector2 pos)
    {
        DropMode mode;
        if (currentController != null)
        {
            var grid = currentController.ScreenToGrid(pos + offset + GetDraggedItemOffset(currentController.inventoryRenderer, Item));

            if (currentController.Inventory.CanAddAt(Item, grid))
            {
                currentController.Inventory.TryAddAt(Item, grid);
                mode = DropMode.Added;
            }
            else if (CanSwap())
            {
                var otherItem = currentController.Inventory.AllItems[0];
                currentController.Inventory.TryRemove(otherItem);
                OriginalController.Inventory.TryAdd(otherItem);
                currentController.Inventory.TryAdd(Item);
                mode = DropMode.Swapped;
            }
            else
            {
                OriginalController.Inventory.TryAddAt(Item, OriginPoint);
                mode = DropMode.Returned;
            }

            currentController.inventoryRenderer.ClearSelection();
        }
        else
        {
            mode = DropMode.Dropped;
            if (!OriginalController.Inventory.TryForceDrop(Item))
            {
                OriginalController.Inventory.TryAddAt(Item, OriginPoint);
            }
        }

        Object.Destroy(image.gameObject);

        return mode;
    }
    private Vector2 GetDraggedItemOffset(InventoryRenderer renderer, IInventoryItem item)
    {
        var scale = new Vector2(
            Screen.width / canvasRect.sizeDelta.x,
            Screen.height / canvasRect.sizeDelta.y
        );
        var gx = -(item.Width * renderer.CellSize.x / 2f) + (renderer.CellSize.x / 2);
        var gy = -(item.Height * renderer.CellSize.y / 2f) + (renderer.CellSize.y / 2);
        return new Vector2(gx, gy) * scale;
    }
    private bool CanSwap()
    {
        if (!currentController.Inventory.CanSwap(Item)) return false;
        var otherItem = currentController.Inventory.AllItems[0];
        return OriginalController.Inventory.CanAdd(otherItem) && currentController.Inventory.CanRemove(otherItem);
    }
}
