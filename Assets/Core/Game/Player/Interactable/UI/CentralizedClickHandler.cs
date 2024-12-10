using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CentralizedClickHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private InventoryController inventoryController;

    private ItemData currentItemData;
    private PointerEventData inventoryEventData;
    private PointerEventData currentEventData;

    public bool IsOpenInvenotry
    {
        set
        {
            if (isOpenInventory != value)
            {
                isOpenInventory = value;
            }
        }
    }
    private bool isOpenInventory;
    public bool IsDropItem
    {
        set
        {
            if (isDropItem != value)
            {
                isDropItem = value;
            }
        }
    }
    private bool isDropItem;

    private void Start()
    {
        if (playerCamera == null) playerCamera = Boostrap.Instance.Camera;

        currentEventData = new PointerEventData(EventSystem.current);
    }

    public void UnsubscribeEvents()
    {
        SubscribeToInventoryEvents(false);
    }
    public void SubscribeEvents()
    {
        SubscribeToInventoryEvents(true);
    }
    private void SubscribeToInventoryEvents(bool subscribe)
    {
        if (inventoryController == null) return;

        if (subscribe)
        {
            inventoryController.OnItemPickedUp += HandleItemAdded;
            inventoryController.OnItemDropped += HandleItemDropped;
        }
        else
        {
            inventoryController.OnItemPickedUp -= HandleItemAdded;
            inventoryController.OnItemDropped -= HandleItemDropped;
        }
    }

    private void Update()
    {
        if (!isOpenInventory) return;
        UpdateGlobalEvenData();
    }

    public void HandleClick()
    {
        if (currentItemData != null)
        {
            if (isDropItem)
            {
                inventoryController.OnEndDrag(currentEventData);
            }
            else
            {
                inventoryController.OnPointerExit(currentEventData);
                inventoryController.OnEndDrag(currentEventData);
                IsDropItem = false;
            }
            currentItemData = null;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TextMeshPro textMeshPro = hit.collider.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                currentItemData = hit.collider.GetComponentInParent<ItemData>();
                if (currentItemData != null && currentItemData.ItemDefinition != null)
                {
                    IInventoryItem item = currentItemData.ItemDefinition.CreateInstance();
                    currentItemData.gameObject.SetActive(false);

                    inventoryController.Inventory.TryAdd(item);
                    inventoryController.OnItemPickedUp?.Invoke(item);
                }
            }
        }
    }
    
    private void UpdateGlobalEvenData()
    {
        currentEventData.position = Input.mousePosition;
        inventoryController.currentEventData = currentEventData;
    }

    private void HandleItemAdded(IInventoryItem addedItem)
    {
        if (addedItem == null) return;

        if (isOpenInventory)
        {
            inventoryController.OnPointerDown(currentEventData);
            inventoryController.itemToDrag = addedItem;
            inventoryController.OnBeginDrag(currentEventData);
        }

        //if (currentItemData != null)
        //{
        //    Destroy(currentItemData.gameObject);
        //    currentItemData = null;
        //}
    }

    private void HandleItemDropped(IInventoryItem droppedItem)
    {
        if (currentItemData != null)
        {
            currentItemData.gameObject.SetActive(true);
            currentItemData = null;
        }
    }
}