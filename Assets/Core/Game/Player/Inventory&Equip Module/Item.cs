using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public ItemType Type;
    public int Row;
    public int Column;

    public virtual void Use()
    {
        Debug.Log($"Use item: {ItemName}");
    }
}
