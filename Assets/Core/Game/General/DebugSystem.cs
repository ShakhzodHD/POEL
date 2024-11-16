using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject debugPanel;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            InventoryItem sword = new("Anus", 1, false, 1, 3);
            InventorySystem.Instance.AddItem(sword);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
}
