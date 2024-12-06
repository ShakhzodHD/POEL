using UnityEngine;

[RequireComponent(typeof(InventoryRenderer))]
public class SizeInventory : MonoBehaviour
{
    public InventoryRenderMode renderMode = InventoryRenderMode.Grid;
    public int maximumAlowedItemCount = -1;
    public ItemType allowedItem = ItemType.Any;

    public int width = 8;
    public int height = 4;
}
