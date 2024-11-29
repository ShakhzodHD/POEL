public interface IInventoryProvider
{
    InventoryRenderMode InventoryRenderMode { get; }
    int InventoryItemCount { get; }
    bool IsInventoryFull { get; }
    IInventoryItem GetInventoryItem(int index);
    bool CanAddInventoryItem(IInventoryItem item);
    bool CanRemoveInventoryItem(IInventoryItem item);
    bool CanDropInventoryItem(IInventoryItem item);
    bool AddInventoryItem(IInventoryItem item);
    bool RemoveInventoryItem(IInventoryItem item);
    bool DropInventoryItem(IInventoryItem item);
}
