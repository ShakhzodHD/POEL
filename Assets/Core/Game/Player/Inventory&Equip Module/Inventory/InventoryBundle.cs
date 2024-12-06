using UnityEngine;

public class InventoryBundle : MonoBehaviour
{
    public SizeInventory mainInventory;
    public SizeInventory[] equipmentItems;

    [SerializeField] private Vector2Int sizeSlotEquip = new(1, 1);

    private readonly InventoryRenderMode equipRenderMode = InventoryRenderMode.Single;
    private int equipMaximumAlowedItemCount;
    public void CreateEquipment(out InventoryProvider[] equipProviders, out InventoryManager[] equipInventoryes)
    {
        equipProviders = new InventoryProvider[equipmentItems.Length];
        equipInventoryes = new InventoryManager[equipmentItems.Length];

        equipMaximumAlowedItemCount = equipmentItems[0].maximumAlowedItemCount;

        for (int i = 0; i < equipmentItems.Length; i++)
        {
            ItemType allowedItem = equipmentItems[i].allowedItem;

            equipProviders[i] = new InventoryProvider(equipRenderMode, equipMaximumAlowedItemCount, allowedItem);
            equipInventoryes[i] = new InventoryManager(equipProviders[i], sizeSlotEquip.x, sizeSlotEquip.y);
        }
    }
    public void CreateMainInventory(out InventoryProvider provider, out InventoryManager inventory)
    {
        provider = new InventoryProvider(mainInventory.renderMode, mainInventory.maximumAlowedItemCount, mainInventory.allowedItem);
        inventory = new InventoryManager(provider, mainInventory.width, mainInventory.height);
    }
    public void SetEquipment(Character character)
    {
        for (int i = 0;i < equipmentItems.Length; i++)
        {
            var inventory = character.Equipments.equipmentManages[i];
            var provider = character.Equipments.equipmentProviders[i];

            equipmentItems[i].gameObject.GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

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

            inventory.OnItemAdded += (item) =>
            {
                Debug.Log($"Item Added {(item as ItemDefinition).Name}");
            };
            inventory.OnItemRemoved += (item) =>
            {
                Debug.Log($"Item removed {(item as ItemDefinition).Name}");
            };
        }
    }
    public void SetMainInventory(Character character)
    {
        var inventory = character.MainInventoryManager;
        var provider = character.MainInventoryProvider;

        mainInventory.gameObject.GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

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
