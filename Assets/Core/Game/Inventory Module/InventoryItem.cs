using UnityEngine;

public class InventoryItem : BaseInventoryItem
{
    public int itemId;
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public InventoryItem(string name, int id, bool stack, int w, int h, TypeSlotEnum type)
    {
        itemId = id;
        itemName = name;
        width = w;
        height = h;
        isStackable = stack;
        typeItem = type;
    }
}
