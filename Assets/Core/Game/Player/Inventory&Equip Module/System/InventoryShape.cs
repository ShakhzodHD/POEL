using System;
using UnityEngine;

[Serializable]
public class InventoryShape
{
    public int Width => width;
    public int Height => height;

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] bool[] shape;
    public InventoryShape(int width, int height)
    {
        this.width = width;
        this.height = height;
        shape = new bool[width * height];
    }
    public InventoryShape(bool[,] shape)
    {
        width = shape.GetLength(0);
        height = shape.GetLength(1);
        this.shape = new bool[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                this.shape[GetIndex(x, y)] = shape[x, y];
            }
        }
    }
    public bool IsPartOfShape(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= width || localPoint.y < 0 || localPoint.y >= height)
        {
            return false;
        }

        var index = GetIndex(localPoint.x, localPoint.y);
        return shape[index];
    }
    private int GetIndex(int x, int y)
    {
        y = (height - 1) - y;
        return x + width * y;
    }
}
