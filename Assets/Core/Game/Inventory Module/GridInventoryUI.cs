using UnityEngine;
public class GridInventoryUI : MonoBehaviour
{
    public GridInventory inventory;

    public float cellSize = 50f;
    public float spacing = 2f;

    public RectTransform dragContainer;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private RectTransform gridContainer;

    public RectTransform itemsContainer;

    private GridEquip gridEquip;

    private InventorySlot[,] slots;
    public void SetInventory(GridInventory newInventory)
    {
        gridEquip = GetComponent<GridEquip>();

        inventory = newInventory;

        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        CreateGrid();

        foreach (var item in inventory.Items)
        {
            CreateItemUI(item);
        }
    }

    public void CreateGrid()
    {
        slots = new InventorySlot[inventory.gridWidth, inventory.gridHeight];

        for (int y = 0; y < inventory.gridHeight; y++)
        {
            for (int x = 0; x < inventory.gridWidth; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, gridContainer);
                RectTransform slotRect = slotObj.GetComponent<RectTransform>();

                slotRect.anchorMin = new Vector2(0, 1);
                slotRect.anchorMax = new Vector2(0, 1);
                slotRect.pivot = new Vector2(0, 1);
                slotRect.anchoredPosition = new Vector2(
                    x * (cellSize + spacing),
                    -y * (cellSize + spacing)
                );
                slotRect.sizeDelta = new Vector2(cellSize, cellSize);

                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.gridPosition = new Vector2Int(x, y);
                slots[x, y] = slot;
            }
        }
    }
    public void HighlightSlots(BaseInventoryItem item, Vector2Int position, bool canPlace)
    {
        ResetAllHighlights();

        for (int x = position.x; x < position.x + item.width && x < inventory.gridWidth; x++)
        {
            for (int y = position.y; y < position.y + item.height && y < inventory.gridHeight; y++)
            {
                if (x >= 0 && y >= 0 && x < inventory.gridWidth && y < inventory.gridHeight)
                {
                    slots[x, y].Highlight(canPlace);
                }
            }
        }
    }

    public void ResetAllHighlights()
    {
        if (slots == null) return;

        for (int x = 0; x < inventory.gridWidth; x++)
        {
            for (int y = 0; y < inventory.gridHeight; y++)
            {
                if (slots[x, y] != null)
                {
                    slots[x, y].ResetHighlight();
                }
            }
        }
    }
    public void CreateItemUI(BaseInventoryItem item)
    {
        GameObject itemObject = Instantiate(itemPrefab, itemsContainer);
        InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();

        RectTransform itemRect = itemObject.GetComponent<RectTransform>();
        itemRect.anchorMin = new Vector2(0, 1);
        itemRect.anchorMax = new Vector2(0, 1);
        itemRect.pivot = new Vector2(0, 1);

        itemUI.Initialize(item, this, gridEquip);
        gridEquip.Initialize(inventory);
    }
    public void UpdateInventoryUI()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.Items)
        {
            CreateItemUI(item);
        }
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(
            x * (cellSize + spacing),
            -y * (cellSize + spacing)
        );
    }

    public Vector2Int GetGridPosition(Vector2 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / (cellSize + spacing)),
            Mathf.FloorToInt(-position.y / (cellSize + spacing))
        );
    }

    private void OnEnable()
    {
        if (inventory != null) inventory.OnPlaceItem += UpdateInventoryUI;
    }
    private void OnDisable()
    {
        if (inventory != null) inventory.OnPlaceItem -= UpdateInventoryUI;
    }
}
