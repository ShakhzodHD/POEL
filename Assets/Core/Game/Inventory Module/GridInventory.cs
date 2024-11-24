using System;
using System.Collections.Generic;
using UnityEngine;

public class GridInventory
{
    public int gridWidth = 12;
    public int gridHeight = 5;

    private bool[,] occupiedCells;
    private List<BaseInventoryItem> items = new();
    public List<BaseInventoryItem> Items => items;

    public event Action OnPlaceItem;

    public event Action<BaseInventoryItem> OnItemRemoved;
    public event Action<BaseInventoryItem> OnItemAdded;
    public void InitializeGrid()
    {
        occupiedCells = new bool[gridWidth, gridHeight];
    }

    public bool CanPlaceItem(BaseInventoryItem item, int x, int y)
    {
        if (x < 0 || y < 0 || x + item.width > gridWidth || y + item.height > gridHeight)
            return false;

        for (int i = x; i < x + item.width; i++)
        {
            for (int j = y; j < y + item.height; j++)
            {
                if (occupiedCells[i, j]) return false;
            }
        }
        return true;
    }

    public bool TryPlaceItem(BaseInventoryItem item)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (CanPlaceItem(item, x, y))
                {
                    PlaceItem(item, x, y);
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceItem(BaseInventoryItem item, int x, int y)
    {
        for (int i = x; i < x + item.width; i++)
        {
            for (int j = y; j < y + item.height; j++)
            {
                occupiedCells[i, j] = true;
            }
        }

        item.position = new Vector2Int(x, y);
        items.Add(item);
        Debug.Log($"Добавлен предмет {item.ToString()}");

        OnPlaceItem?.Invoke();
    }

    public void RemoveItem(BaseInventoryItem item)
    {
        if (item.position.x < 0) return;

        for (int i = item.position.x; i < item.position.x + item.width; i++)
        {
            for (int j = item.position.y; j < item.position.y + item.height; j++)
            {
                occupiedCells[i, j] = false;
            }
        }

        items.Remove(item);
        item.position = new Vector2Int(-1, -1);
    }
    public bool CanFindSpaceForItem(BaseInventoryItem item)
    {
        // Проверяем каждую позицию в сетке
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (CanPlaceItem(item, x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
