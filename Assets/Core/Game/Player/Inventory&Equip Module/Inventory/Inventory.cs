using System;

public class Inventory
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Item[,] grid; //temp public for debug system

    public Inventory(int column, int row)
    {
        if (column <= 0 || row <= 0)
            throw new ArgumentException("Invalid dimensions for inventory.");

        Width = row;
        Height = column;
        grid = new Item[Width, Height];
    }

    public bool TryPlaceItem(Item item)
    {
        for (int x = 0; x <= Width - item.Row; x++)
        {
            for (int y = 0; y <= Height - item.Column; y++)
            {
                if (CheckSpace(item, x, y))
                {
                    PlaceItem(item, x, y);
                    return true;
                }
            }
        }
        return false;   
    }

    public bool PlaceItem(Item item, int x, int y)
    {
        if (!CheckSpace(item, x, y))
            return false;

        for (int i = 0; i < item.Column; i++)
        {
            for (int j = 0; j < item.Row; j++)
            {
                grid[x + j, y + i] = item;
            }
        }
        return true;
    }

    public void RemoveItem(Item item)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (grid[x, y] == item)
                    grid[x, y] = null;
            }
        }
    }
    public bool CheckSpace(Item item, int x, int y)
    {
        if (x + item.Row > Width || y + item.Column > Height)
            return false;

        for (int i = 0; i < item.Column; i++)
        {
            for (int j = 0; j < item.Row; j++)
            {
                if (grid[x + j, y + i] != null)
                    return false;
            }
        }
        return true;
    }

}
