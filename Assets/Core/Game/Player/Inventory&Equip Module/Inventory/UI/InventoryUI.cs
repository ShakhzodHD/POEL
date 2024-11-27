using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private GameObject prefabSlot;
    [SerializeField] private GameObject prefabItem;
    [SerializeField] private RectTransform containerSlots;
    [SerializeField] private RectTransform containerItems;

    public void CreateGridInventory()
    {
        for (int i = 0; i < containerSlots.childCount; i++)
        {
            Destroy(containerSlots.GetChild(i).gameObject);
        }

        int width = inventoryManager.PlayerInventory.Width;
        int height = inventoryManager.PlayerInventory.Height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject slot = Instantiate(prefabSlot,containerSlots);
                slot.GetComponentInChildren<Text>().text = i.ToString() + "," + j;
            }
        }
    }
    public void CreateItemUI(Item item)
    {
        var itemUI = Instantiate(prefabItem, containerItems).GetComponent<InventoryItemUI>();
        itemUI.Initialize(item, ref inventoryManager);
    }
    public void RefreshGrid()
    {
        var grid = inventoryManager.PlayerInventory.grid;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var item = grid[i, j];
                if (item != null)
                {
                    int index = i * grid.GetLength(1) + j;
                    containerSlots.GetChild(index).GetComponent<Image>().color = Color.red;
                }
            }
        }
    }
}
