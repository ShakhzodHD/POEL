using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Item currentItem;

    private int column;
    private int row;

    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private InventoryManager inventoryManager;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Initialize(Item item, ref InventoryManager manager)
    {
        currentItem = item;

        column = item.Column;
        row = item.Row;

        inventoryManager = manager;

        SetStartTransform();
    }
    private void SetStartTransform()
    {
        if (column <= 0 || row <= 0)
        {
            return;
        }

        rectTransform.sizeDelta = new Vector2(50 * column, 50 * row);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        originalPosition = rectTransform.anchoredPosition;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        //rectTransform.anchoredPosition = originalPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Delete");
        }
    }
}
