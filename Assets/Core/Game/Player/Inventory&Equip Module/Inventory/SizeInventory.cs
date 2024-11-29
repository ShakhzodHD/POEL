using UnityEngine;

[RequireComponent(typeof(InventoryRenderer))]
public class SizeInventory : MonoBehaviour
{
    [SerializeField] private InventoryRenderMode renderMode = InventoryRenderMode.Grid;
    [SerializeField] private int maximumAlowedItemCount = -1;
    [SerializeField] private ItemType allowedItem = ItemType.Any;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 4;
    [SerializeField] private ItemDefinition[] _definitions = null;
    [SerializeField] private bool fillRandomly = true;
    [SerializeField] private bool fillEmpty = false;
    private void Start()
    {
        var provider = new InventoryProvider(renderMode, maximumAlowedItemCount, allowedItem);
        var inventory = new InventoryManager(provider, width, height);

        if (fillRandomly)
        {
            var tries = (width * height) / 3;
            for (var i = 0; i < tries; i++)
            {
                inventory.TryAdd(_definitions[Random.Range(0, _definitions.Length)].CreateInstance());
            }
        }
        if (fillEmpty)
        {
            for (var i = 0; i < width * height; i++)
            {
                inventory.TryAdd(_definitions[0].CreateInstance());
            }
        }

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
