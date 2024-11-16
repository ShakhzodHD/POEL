using UnityEngine;
//singlton, awaiting refactoring and integration with the rest of the system
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    [SerializeField] private GridInventoryUI inventoryUI;
    [SerializeField] private GridInventory inventory;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void AddItem(InventoryItem item)
    {
        if (inventory.TryPlaceItem(item))
        {
            Debug.Log("Succsess");
            inventoryUI.CreateItemUI(item);
        }
        else
        {
            Debug.LogError("No space in inventory.");
        }
    }
    public void RemoveItem(GameObject obj, BaseInventoryItem item)
    {
        inventory.RemoveItem(item);
        Destroy(obj);
    }
}
