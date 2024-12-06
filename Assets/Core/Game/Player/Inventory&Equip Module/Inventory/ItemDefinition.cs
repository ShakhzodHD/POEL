using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class ItemDefinition : ScriptableObject, IInventoryItem
{
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private InventoryShape shape = null;
    [SerializeField] private ItemType type = ItemType.Any;
    [SerializeField] private bool canDrop = true;
    [SerializeField, HideInInspector] private Vector2Int position = Vector2Int.zero;

    public string Name => name;
    public ItemType Type => type;
    public Sprite Sprite => sprite;
    public int Width => shape.Width;
    public int Height => shape.Height;
    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }
    public bool CanDrop => canDrop;
    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return shape.IsPartOfShape(localPosition);
    }
    public IInventoryItem CreateInstance()
    {
        var clone = Instantiate(this);
        clone.name = clone.name.Substring(0, clone.name.Length - 7);
        return clone;
    }
}
