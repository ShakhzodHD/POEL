using UnityEngine;

public class InventoryBundle : MonoBehaviour
{
    public SizeInventory mainInventory;
    public SizeInventory[] equipmentItems;
    public SizeInventory stash;

    [SerializeField] private Vector2Int sizeSlotEquip = new(1, 1);

    private readonly InventoryRenderMode equipRenderMode = InventoryRenderMode.Single;
    private int equipMaximumAlowedItemCount;

    private InventoryManager currentInventory;
    private InventoryProvider currentProvider;

    private InventoryManager[] equipmentInventories;

    private Stash currentStash;

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

        equipmentInventories = equipInventoryes;
    }

    public void CreateMainInventory(out InventoryProvider provider, out InventoryManager inventory)
    {
        provider = new InventoryProvider(mainInventory.renderMode, mainInventory.maximumAlowedItemCount, mainInventory.allowedItem);
        inventory = new InventoryManager(provider, mainInventory.width, mainInventory.height);
    }

    public void CreateStash(out InventoryProvider provider, out InventoryManager inventory)
    {
        provider = new InventoryProvider(stash.renderMode);
        inventory = new InventoryManager(provider, stash.width, stash.height);
    }

    public void SetEquipment(Character character)
    {
        if (equipmentInventories != null)
        {
            foreach (var inventory in equipmentInventories)
            {
                UnsubscribeFromInventoryEvents(inventory);
            }
        }

        for (int i = 0; i < equipmentItems.Length; i++)
        {
            var inventory = character.Equipments.equipmentManages[i];
            var provider = character.Equipments.equipmentProviders[i];

            equipmentItems[i].gameObject.GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

            SubscribeToInventoryEvents(inventory);
        }
    }

    public void SetMainInventory(Character character)
    {
        if (currentInventory != null)
        {
            UnsubscribeFromInventoryEvents(currentInventory);
        }

        currentInventory = character.MainInventoryManager;
        currentProvider = character.MainInventoryProvider;

        mainInventory.gameObject.GetComponent<InventoryRenderer>().SetInventory(currentInventory, currentProvider.InventoryRenderMode);

        SubscribeToInventoryEvents(currentInventory);
    }

    public void SetStash(Character character)
    {
        var stashManager = character.Stash.stashManager;
        var stashProvider = character.Stash.stashProvider;

        stash.gameObject.GetComponent<InventoryRenderer>().SetInventory(stashManager,stashProvider.InventoryRenderMode);
    }

    private void SubscribeToInventoryEvents(InventoryManager inventory)
    {
        inventory.OnItemDropped += OnItemDropped;
        inventory.OnItemDroppedFailed += OnItemDroppedFailed;
        inventory.OnItemAddedFailed += OnItemAddedFailed;
        inventory.OnItemAdded += OnItemAdded;
        inventory.OnItemRemoved += OnItemRemoved;
    }

    private void UnsubscribeFromInventoryEvents(InventoryManager inventory)
    {
        inventory.OnItemDropped -= OnItemDropped;
        inventory.OnItemDroppedFailed -= OnItemDroppedFailed;
        inventory.OnItemAddedFailed -= OnItemAddedFailed;
        inventory.OnItemAdded -= OnItemAdded;
        inventory.OnItemRemoved -= OnItemRemoved;
    }

    private void OnItemDropped(object item)
    {
        //Debug.Log((item as ItemDefinition).Name + " was dropped on the ground");
        Boostrap.Instance.InteractionManager.DropItem(item as ItemDefinition);
    }

    private void OnItemDroppedFailed(object item)
    {
        Debug.Log($"You're not allowed to drop {(item as ItemDefinition).Name} on the ground");
    }

    private void OnItemAddedFailed(object item)
    {
        Boostrap.Instance.InteractionManager.DropItem(item as ItemDefinition);
        Debug.Log($"You can't put {(item as ItemDefinition).Name} there!");
    }

    private void OnItemAdded(object item)
    {
        Debug.Log($"Item Added {(item as ItemDefinition).Name}");
    }

    private void OnItemRemoved(object item)
    {
        Debug.Log($"Item removed {(item as ItemDefinition).Name}");
    }
}
