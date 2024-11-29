using System;

public interface IInventoryController
{
    Action<IInventoryItem> OnItemHovered { get; set; }
    Action<IInventoryItem> OnItemPickedUp { get; set; }
    Action<IInventoryItem> OnItemAdded { get; set; }
    Action<IInventoryItem> OnItemSwapped { get; set; }
    Action<IInventoryItem> OnItemReturned { get; set; }
    Action<IInventoryItem> OnItemDropped { get; set; }
}
