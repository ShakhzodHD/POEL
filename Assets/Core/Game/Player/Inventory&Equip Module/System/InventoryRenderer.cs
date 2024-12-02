using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventoryRenderer : MonoBehaviour
{
    [SerializeField] private Vector2Int cellSize = new Vector2Int(32, 32);
    [SerializeField] private Sprite cellSpriteEmpty = null;
    [SerializeField] private Sprite cellSpriteSelected = null;
    [SerializeField] private Sprite cellSpriteBlocked = null;

    public IInventoryManager inventory;
    InventoryRenderMode renderMode;

    private bool haveListeners;
    private Pool<Image> imagePool;
    private Image[] grids;
    private Dictionary<IInventoryItem, Image> items = new();

    public RectTransform RectTransform { get; private set; }
    public Vector2 CellSize => cellSize;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();

        var imageContainer = new GameObject("ImagePool").AddComponent<RectTransform>();
        imageContainer.transform.SetParent(transform);
        imageContainer.transform.localPosition = Vector3.zero;
        imageContainer.transform.localScale = Vector3.one;

        imagePool = new Pool<Image>(
            delegate
            {
                var image = new GameObject("Image").AddComponent<Image>();
                image.transform.SetParent(imageContainer);
                image.transform.localScale = Vector3.one;
                return image;
            });
    }
    public void SetInventory(IInventoryManager inventoryManager, InventoryRenderMode renderMode)
    {
        OnDisable();
        inventory = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
        this.renderMode = renderMode;
    }
    private void OnEnable()
    {
        if (inventory != null && !haveListeners)
        {
            if (cellSpriteEmpty == null) { throw new NullReferenceException("Sprite for empty cell is null"); }
            if (cellSpriteSelected == null) { throw new NullReferenceException("Sprite for selected cells is null."); }
            if (cellSpriteBlocked == null) { throw new NullReferenceException("Sprite for blocked cells is null."); }

            inventory.OnRebuilt += ReRenderAllItems;
            inventory.OnItemAdded += HandleItemAdded;
            inventory.OnItemRemoved += HandleItemRemoved;
            inventory.OnItemDropped += HandleItemRemoved;
            inventory.OnResized += HandleResized;
            haveListeners = true;

            ReRenderGrid();
            ReRenderAllItems();
        }
    }
    private void OnDisable()
    {
        if (inventory != null && haveListeners)
        {
            inventory.OnRebuilt -= ReRenderAllItems;
            inventory.OnItemAdded -= HandleItemAdded;
            inventory.OnItemRemoved -= HandleItemRemoved;
            inventory.OnItemDropped -= HandleItemRemoved;
            inventory.OnResized -= HandleResized;
            haveListeners = false;
        }
    }
    private void ReRenderGrid()
    {
        if (grids != null)
        {
            for (var i = 0; i < grids.Length; i++)
            {
                grids[i].gameObject.SetActive(false);
                RecycleImage(grids[i]);
                grids[i].transform.SetSiblingIndex(i);
            }
        }
        grids = null;

        var containerSize = new Vector2(cellSize.x * inventory.Width, cellSize.y * inventory.Height);
        Image grid;

        switch (renderMode)
        {
            case InventoryRenderMode.Single:
                grid = CreateImage(cellSpriteEmpty, true);
                grid.rectTransform.SetAsFirstSibling();
                grid.type = Image.Type.Sliced;
                grid.rectTransform.localPosition = Vector3.zero;
                grid.rectTransform.sizeDelta = containerSize;
                grids = new[] { grid };
                break;
            default:
                var topLeft = new Vector3(-containerSize.x / 2, -containerSize.y / 2, 0);
                var halfCellSize = new Vector3(cellSize.x / 2, cellSize.y / 2, 0);
                grids = new Image[inventory.Width * inventory.Height];
                var c = 0;
                for (int y = 0; y < inventory.Height; y++)
                {
                    for (int x = 0; x < inventory.Width; x++)
                    {
                        grid = CreateImage(cellSpriteEmpty, true);
                        grid.gameObject.name = "Grid " + c;
                        grid.rectTransform.SetAsFirstSibling();
                        grid.type = Image.Type.Sliced;
                        grid.rectTransform.localPosition = topLeft + new Vector3(cellSize.x * ((inventory.Width - 1) - x), cellSize.y * y, 0) + halfCellSize;
                        grid.rectTransform.sizeDelta = cellSize;
                        grids[c] = grid;
                        c++;
                    }
                }
                break;
        }
        RectTransform.sizeDelta = containerSize;
    }
    private void ReRenderAllItems()
    {
        foreach (var image in items.Values)
        {
            image.gameObject.SetActive(false);
            RecycleImage(image);
        }
        items.Clear();

        foreach (var item in inventory.AllItems)
        {
            HandleItemAdded(item);
        }
    }
    private void HandleItemAdded(IInventoryItem item)
    {
        var img = CreateImage(item.Sprite, false);

        if (renderMode == InventoryRenderMode.Single)
        {
            img.rectTransform.localPosition = RectTransform.rect.center;
        }
        else
        {
            img.rectTransform.localPosition = GetItemOffset(item);
        }

        items.Add(item, img);
    }
    private void HandleItemRemoved(IInventoryItem item)
    {
        if (items.ContainsKey(item))
        {
            var image = items[item];
            image.gameObject.SetActive(false);
            RecycleImage(image);
            items.Remove(item);
        }
    }
    private void HandleResized()
    {
        ReRenderGrid();
        ReRenderAllItems();
    }
    private Image CreateImage(Sprite sprite, bool raycastTarget)
    {
        var img = imagePool.Take();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling();
        img.type = Image.Type.Simple;
        img.raycastTarget = raycastTarget;
        return img;
    }
    private void RecycleImage(Image image)
    {
        image.gameObject.name = "Image";
        image.gameObject.SetActive(false);
        imagePool.Recycle(image);
    }
    public void SelectItem(IInventoryItem item, bool blocked, Color color)
    {
        if (item == null) { return; }
        ClearSelection();

        switch (renderMode)
        {
            case InventoryRenderMode.Single:
                
                grids[0].sprite = blocked ? cellSpriteBlocked : cellSpriteSelected;
                grids[0].color = color;
                break;
            default:
                for (var x = 0; x < item.Width; x++)
                {
                    for (var y = 0; y < item.Height; y++)
                    {
                        if (item.IsPartOfShape(new Vector2Int(x, y)))
                        {
                            var p = item.Position + new Vector2Int(x, y);
                            if (p.x >= 0 && p.x < inventory.Width && p.y >= 0 && p.y < inventory.Height)
                            {
                                var index = p.y * inventory.Width + ((inventory.Width - 1) - p.x);
                                grids[index].sprite = blocked ? cellSpriteBlocked : cellSpriteSelected;
                                grids[index].color = color;
                            }
                        }
                    }
                }
                break;
        }
    }
    public void ClearSelection()
    {
        for (var i = 0; i < grids.Length; i++)
        {
            grids[i].sprite = cellSpriteEmpty;
            grids[i].color = Color.white;
        }
    }

    internal Vector2 GetItemOffset(IInventoryItem item)
    {
        var x = (-(inventory.Width * 0.5f) + item.Position.x + item.Width * 0.5f) * cellSize.x;
        var y = (-(inventory.Height * 0.5f) + item.Position.y + item.Height * 0.5f) * cellSize.y;
        return new Vector2(x, y);
    }
}
