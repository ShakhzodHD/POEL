using UnityEngine;
using UnityEngine.UI;

public class InventorySelection : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = string.Empty;

        var allControllers = GameObject.FindObjectsOfType<InventoryController>();

        foreach (var controller in allControllers)
        {
            controller.OnItemHovered += HandleItemHover;
        }
    }

    private void HandleItemHover(IInventoryItem item)
    {
        if (item != null)
        {
            text.text = (item as ItemDefinition).Name;
        }
        else
        {
            text.text = string.Empty;
        }
    }
}
