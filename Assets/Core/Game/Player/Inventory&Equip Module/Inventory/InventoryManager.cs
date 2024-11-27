using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryUI inventoryUI;

    public Inventory PlayerInventory;
    public Equipment PlayerEquipment;

    public void Initialize(Inventory newInventory, Equipment newEquipment)
    {
        PlayerInventory = newInventory;
        PlayerEquipment = newEquipment;

        inventoryUI.CreateGridInventory();
    }
    public void TryPlaceItem(Item item)
    {
        if (PlayerInventory.TryPlaceItem(item))
        {
            inventoryUI.CreateItemUI(item);
            inventoryUI.RefreshGrid();
        }
        else
        {
            Debug.Log("Not enough space");
        }
    }
    public void EquipItemFromInventory(Item item)
    {
        if (PlayerEquipment.EquipItem(item))
        {
            PlayerInventory.RemoveItem(item);
        }
    }

    public void UnequipItemToInventory(ItemType itemType)
    {
        var item = PlayerEquipment.GetEquippedItem(itemType);
        //if (item != null && PlayerInventory.AddItem(item, 0, 0))
        //{
        //    PlayerEquipment.UnequipItem(itemType);
        //}
    }
}
