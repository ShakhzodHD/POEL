using System;
using System.Linq;
using UnityEngine;

public class InventoryManager : IInventoryManager
{
    private Vector2Int size = Vector2Int.one;
    private IInventoryProvider provider;
    private Rect fullRect;

    public int Width => size.x;
    public int Height => size.y;

    public IInventoryItem[] AllItems { get; private set; }
    public Action OnRebuilt { get; set; }
    public Action<IInventoryItem> OnItemDropped { get; set; }
    public Action<IInventoryItem> OnItemDroppedFailed { get; set; }
    public Action<IInventoryItem> OnItemAdded { get; set; }
    public Action<IInventoryItem> OnItemAddedFailed { get; set; }
    public Action<IInventoryItem> OnItemRemoved { get; set; }
    public Action OnResized { get; set; }
    public bool IsFull
    {
        get
        {
            if (provider.IsInventoryFull) return true;

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (GetAtPoint(new Vector2Int(x, y)) == null) { return false; }
                }
            }
            return true;
        }
    }

    public InventoryManager(IInventoryProvider provider, int width, int height)
    {
        this.provider = provider;
        Rebuild();
        Resize(width, height);
    }
    public void Resize(int newWidth, int newHeight)
    {
        size.x = newWidth;
        size.y = newHeight;
        RebuildRect();
    }
    private void RebuildRect()
    {
        fullRect = new Rect(0, 0, size.x, size.y);
        HandleSizeChanged();
        OnResized?.Invoke();
    }
    private void HandleSizeChanged()
    {
        for (int i = 0; i < AllItems.Length;)
        {
            var item = AllItems[i];
            var shouldBeDropped = false;
            var padding = Vector2.one * 0.01f;

            if (!fullRect.Contains(item.GetMinPoint() + padding) || !fullRect.Contains(item.GetMaxPoint() - padding))
            {
                shouldBeDropped = true;
            }

            if (shouldBeDropped)
            {
                TryDrop(item);
            }
            else
            {
                i++;
            }
        }
    }
    public void Rebuild()
    {
        Rebuild(false);
    }
    private void Rebuild(bool silent)
    {
        AllItems = new IInventoryItem[provider.InventoryItemCount];
        for (var i = 0; i < provider.InventoryItemCount; i++)
        {
            AllItems[i] = provider.GetInventoryItem(i);
        }
        if (!silent) OnRebuilt?.Invoke();
    }
    public void Dispose()
    {
        provider = null;
        AllItems = null;
    }
    public IInventoryItem GetAtPoint(Vector2Int point)
    {
        if (provider.InventoryRenderMode == InventoryRenderMode.Single && provider.IsInventoryFull && AllItems.Length > 0)
        {
            return AllItems[0];
        }

        foreach (var item in AllItems)
        {
            if (item.Contains(point)) { return item; }
        }
        return null;
    }
    public IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size)
    {
        var posibleItems = new IInventoryItem[size.x * size.y];
        var c = 0;
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                posibleItems[c] = GetAtPoint(point + new Vector2Int(x, y));
                c++;
            }
        }
        return posibleItems.Distinct().Where(x => x != null).ToArray();
    }
    public bool TryRemove(IInventoryItem item)
    {
        if (!CanRemove(item)) return false;
        if (!provider.RemoveInventoryItem(item)) return false;
        Rebuild(true);
        OnItemRemoved?.Invoke(item);
        return true;
    }
    public bool TryDrop(IInventoryItem item)
    {
        if (!CanDrop(item) || !provider.DropInventoryItem(item))
        {
            OnItemDroppedFailed?.Invoke(item);
            return false;
        }
        Rebuild(true);
        OnItemDropped?.Invoke(item);
        return true;
    }
    public bool TryForceDrop(IInventoryItem item)
    {
        if (!item.CanDrop)
        {
            OnItemDroppedFailed?.Invoke(item);
            return false;
        }
        OnItemDropped?.Invoke(item);
        return true;
    }
    public bool CanAddAt(IInventoryItem item, Vector2Int point)
    {
        if (!provider.CanAddInventoryItem(item) || provider.IsInventoryFull)
        {
            return false;
        }

        if (provider.InventoryRenderMode == InventoryRenderMode.Single)
        {
            return true;
        }

        var previousPoint = item.Position;
        item.Position = point;
        var padding = Vector2.one * 0.01f;

        if (!fullRect.Contains(item.GetMinPoint() + padding) || !fullRect.Contains(item.GetMaxPoint() - padding))
        {
            item.Position = previousPoint;
            return false;
        }

        if (!AllItems.Any(otherItem => item.Overlaps(otherItem))) return true;
        item.Position = previousPoint;
        return false;
    }
    public bool TryAddAt(IInventoryItem item, Vector2Int point)
    {
        if (!CanAddAt(item, point) || !provider.AddInventoryItem(item))
        {
            OnItemAddedFailed?.Invoke(item);
            return false;
        }
        switch (provider.InventoryRenderMode)
        {
            case InventoryRenderMode.Single:
                item.Position = GetCenterPosition(item);
                break;
            case InventoryRenderMode.Grid:
                item.Position = point;
                break;
            default:
                throw new NotImplementedException($"InventoryRenderMode.{provider.InventoryRenderMode.ToString()} have not yet been implemented");
        }
        Rebuild(true);
        OnItemAdded?.Invoke(item);
        return true;
    }
    public bool CanAdd(IInventoryItem item)
    {
        Vector2Int point;
        if (!Contains(item) && GetFirstPointThatFitsItem(item, out point))
        {
            return CanAddAt(item, point);
        }
        return false;
    }
    public bool TryAdd(IInventoryItem item)
    {
        if (!CanAdd(item)) return false;
        Vector2Int point;
        return GetFirstPointThatFitsItem(item, out point) && TryAddAt(item, point);
    }
    public bool CanSwap(IInventoryItem item)
    {
        return provider.InventoryRenderMode == InventoryRenderMode.Single &&
            DoesItemFit(item) &&
            provider.CanAddInventoryItem(item);
    }
    public void DropAll()
    {
        var itemsToDrop = AllItems.ToArray();
        foreach (var item in itemsToDrop)
        {
            TryDrop(item);
        }
    }
    public void Clear()
    {
        foreach (var item in AllItems)
        {
            TryRemove(item);
        }
    }
    public bool Contains(IInventoryItem item) => AllItems.Contains(item);
    public bool CanRemove(IInventoryItem item) => Contains(item) && provider.CanRemoveInventoryItem(item);
    public bool CanDrop(IInventoryItem item) => Contains(item) && provider.CanDropInventoryItem(item) && item.CanDrop;
    private bool GetFirstPointThatFitsItem(IInventoryItem item, out Vector2Int point)
    {
        if (DoesItemFit(item))
        {
            for (var x = 0; x < Width - (item.Width - 1); x++)
            {
                for (var y = 0; y < Height - (item.Height - 1); y++)
                {
                    point = new Vector2Int(x, y);
                    if (CanAddAt(item, point)) return true;
                }
            }
        }
        point = Vector2Int.zero;
        return false;
    }
    private bool DoesItemFit(IInventoryItem item) => item.Width <= Width && item.Height <= Height;
    private Vector2Int GetCenterPosition(IInventoryItem item)
    {
        return new Vector2Int(
            (size.x - item.Width) / 2,
            (size.y - item.Height) / 2
        );
    }
}
