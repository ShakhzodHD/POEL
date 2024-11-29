using UnityEngine;

public interface IInventoryItem
{
    Sprite Sprite { get;}
    Vector2Int Position { get; set; }
    int Width { get; }
    int Height { get; }
    bool IsPartOfShape(Vector2Int localPosition);
    bool CanDrop { get; }
}
internal static class InventoryItemExtensions
{
    internal static Vector2Int GetMinPoint(this IInventoryItem item)
    {
        return item.Position;
    }
    internal static Vector2Int GetMaxPoint(this IInventoryItem item)
    {
        return item.Position + new Vector2Int(item.Width, item.Height);
    }
    internal static bool Contains(this IInventoryItem item, Vector2Int inventoryPoint)
    {
        for (var iX = 0; iX < item.Width; iX++)
        {
            for (var iY = 0; iY < item.Height; iY++)
            {
                var iPoint = item.Position + new Vector2Int(iX, iY);
                if (iPoint == inventoryPoint) { return true; }
            }
        }
        return false;
    }
    internal static bool Overlaps(this IInventoryItem item, IInventoryItem otherItem)
    {
        for (var iX = 0; iX < item.Width; iX++)
        {
            for (var iY = 0; iY < item.Height; iY++)
            {
                if (item.IsPartOfShape(new Vector2Int(iX, iY)))
                {
                    var iPoint = item.Position + new Vector2Int(iX, iY);
                    for (var oX = 0; oX < otherItem.Width; oX++)
                    {
                        for (var oY = 0; oY < otherItem.Height; oY++)
                        {
                            if (otherItem.IsPartOfShape(new Vector2Int(oX, oY)))
                            {
                                var oPoint = otherItem.Position + new Vector2Int(oX, oY);
                                if (oPoint == iPoint) { return true; }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}