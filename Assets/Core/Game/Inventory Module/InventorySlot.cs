using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image background;

    public Vector2Int gridPosition;

    private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color highlightColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    private Color invalidColor = new Color(0.7f, 0.2f, 0.2f, 1f);

    private void Awake()
    {
        if (background == null) background = GetComponent<Image>();
        background.color = normalColor;
    }

    public void Highlight(bool canPlace)
    {
        background.color = canPlace ? highlightColor : invalidColor;
    }

    public void ResetHighlight()
    {
        background.color = normalColor;
    }
}
