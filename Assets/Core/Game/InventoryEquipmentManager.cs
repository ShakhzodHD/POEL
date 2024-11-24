using UnityEngine;

public class InventoryEquipmentManager : MonoBehaviour
{
    [SerializeField] private GridInventoryUI _inventoryUI;
    
    [SerializeField] private GridEquip _equipSystem;

    private GridInventory _inventory;

    public void Initialize(GridInventory inventory)
    {
        _inventory = inventory;

        // �������������� �������
        _inventoryUI.SetInventory(inventory);
        _equipSystem.Initialize(inventory);

        // ������������� �� �������
        _inventory.OnPlaceItem += OnInventoryUpdated;
    }

    private void OnInventoryUpdated()
    {
        // ��������� ��� ������������� ��������
        foreach (var item in _inventory.Items)
        {
            var itemUI = GetItemUI(item);
            if (itemUI != null && itemUI.isEquipped)
            {
                // ��������� ���������� ���������
                itemUI.UpdateVisuals();
            }
        }
    }

    // ������� ����������� �������
    public bool TryEquipItem(BaseInventoryItem item)
    {
        if (_equipSystem.IsEquipped(item))
        {
            return false;
        }

        _inventory.RemoveItem(item);
        _equipSystem.EquipItem(item);
        return true;
    }

    // ������� ����� �������
    public bool TryUnequipItem(BaseInventoryItem item)
    {
        if (!_equipSystem.IsEquipped(item))
        {
            return false;
        }

        if (_inventory.TryPlaceItem(item))
        {
            _equipSystem.UnequipItem(item);
            return true;
        }

        return false;
    }

    private InventoryItemUI GetItemUI(BaseInventoryItem item)
    {
        foreach (Transform child in _inventoryUI.itemsContainer)
        {
            var itemUI = child.GetComponent<InventoryItemUI>();
            if (itemUI != null && itemUI.GetItem() == item)
            {
                return itemUI;
            }
        }
        
        return null;
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnPlaceItem -= OnInventoryUpdated;
        }
    }
}