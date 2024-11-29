using System;
using UnityEngine;

public interface IInventoryManager : IDisposable
{
    Action<IInventoryItem> OnItemAdded { get; set; }
    Action<IInventoryItem> OnItemAddedFailed { get; set; }
    Action<IInventoryItem> OnItemRemoved { get; set; }
    Action<IInventoryItem> OnItemDropped { get; set; }
    Action<IInventoryItem> OnItemDroppedFailed { get; set; }
    Action OnRebuilt { get; set; }
    Action OnResized { get; set; }
    int Width { get; }
    int Height { get; }
    void Resize(int width, int height);
    IInventoryItem[] AllItems { get; }
    bool Contains(IInventoryItem item);
    bool IsFull { get; }
    bool CanAdd(IInventoryItem item);
    bool TryAdd(IInventoryItem item);
    bool CanAddAt(IInventoryItem item, Vector2Int point);
    bool TryAddAt(IInventoryItem item, Vector2Int point);
    bool CanRemove(IInventoryItem item);
    bool CanSwap(IInventoryItem item);
    bool TryRemove(IInventoryItem item);
    bool CanDrop(IInventoryItem item);
    bool TryDrop(IInventoryItem item);
    void DropAll();
    void Clear();
    void Rebuild();
    IInventoryItem GetAtPoint(Vector2Int point);
    IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size);
}
