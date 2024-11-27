public class EquipmentSlot
{
    public ItemType AllowedType;
    public Item EquippedItem;

    public bool Equip(Item item)
    {
        if (item.Type == AllowedType)
        {
            EquippedItem = item;
            return true;
        }
        return false;
    }

    public void Unequip()
    {
        EquippedItem = null;
    }
}
