using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CentralizedClickHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private Canvas canvas;

    private ItemData currentItemData;
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

    private void Start()
    {
        if (playerCamera == null) playerCamera = Boostrap.Instance.Camera;

        currentEventData = new PointerEventData(EventSystem.current);

        SubscribeToInventoryEvents(true);
    }

    private void OnDestroy()
    {
        SubscribeToInventoryEvents(false);
    }
    private void SubscribeToInventoryEvents(bool subscribe)
    {
        if (inventoryController == null) return;

        if (subscribe)
        {
            inventoryController.OnItemAdded += HandleItemAdded;
            inventoryController.OnItemDropped += HandleItemDropped;
        }
        else
        {
            inventoryController.OnItemAdded -= HandleItemAdded;
            inventoryController.OnItemDropped -= HandleItemDropped;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        UpdateGlobalEvenData();
    }

    private void HandleClick()
    {
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
                    inventoryController.OnItemAdded?.Invoke(item);
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

        if (currentItemData != null)
        {
            Destroy(currentItemData.gameObject);
            currentItemData = null;
        }
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