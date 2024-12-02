using UnityEngine;

[RequireComponent(typeof(InventoryRenderer))]
public class SizeInventory : MonoBehaviour
{
    public InventoryRenderMode renderMode = InventoryRenderMode.Grid;
    public int maximumAlowedItemCount = -1;
    public ItemType allowedItem = ItemType.Any;

    public int width = 8;
    public int height = 4;
    public void SetInventory(InventoryProvider provider, InventoryManager inventory)
    {
        GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

        inventory.OnItemDropped += (item) =>
        {
            Debug.Log((item as ItemDefinition).Name + " was dropped on the ground");
        };

        inventory.OnItemDroppedFailed += (item) =>
        {
            Debug.Log($"You're not allowed to drop {(item as ItemDefinition).Name} on the ground");
        };

        inventory.OnItemAddedFailed += (item) =>
        {
            Debug.Log($"You can't put {(item as ItemDefinition).Name} there!");
        };
    }
}
