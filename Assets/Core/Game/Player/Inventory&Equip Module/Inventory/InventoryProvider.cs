using System.Collections.Generic;

public class InventoryProvider : IInventoryProvider
{
    private List<IInventoryItem> items = new();
    private int maximumAlowedItemCount;
    ItemType allowedItem;

    public int InventoryItemCount => items.Count;
    public InventoryRenderMode InventoryRenderMode { get; private set; }
    public bool IsInventoryFull
    {
        get
        {
            if (maximumAlowedItemCount < 0) return false;
            return InventoryItemCount >= maximumAlowedItemCount;
        }
    }
    public InventoryProvider(InventoryRenderMode renderMode, int maximumAlowedItemCount = -1, ItemType allowedItem = ItemType.Any)
    {
        InventoryRenderMode = renderMode;
        this.maximumAlowedItemCount = maximumAlowedItemCount;
        this.allowedItem = allowedItem;
    }
    public bool AddInventoryItem(IInventoryItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            return true;
        }
        return false;
    }
    public bool DropInventoryItem(IInventoryItem item)
    {
        return RemoveInventoryItem(item);
    }

    public IInventoryItem GetInventoryItem(int index)
    {
        return items[index];
    }

    public bool CanAddInventoryItem(IInventoryItem item)
    {
        if (allowedItem == ItemType.Any) return true;
        return (item as ItemDefinition).Type == allowedItem;
    }
    public bool CanRemoveInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool CanDropInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool RemoveInventoryItem(IInventoryItem item)
    {
        return items.Remove(item);
    }
}
