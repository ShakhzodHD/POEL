using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private WeaponItem weaponItem;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            inventoryManager.TryPlaceItem(weaponItem);
        }
    }
}
