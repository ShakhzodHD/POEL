using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            InventoryItem sword = new("Anus", 1, false, 1, 3, TypeSlotEnum.Weapon);
            Boostrap.Instance.PlayerData.selectedCharacter.AddItemToInventory(sword);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            InventoryItem sword = new("Anus2", 2, false, 1, 3, TypeSlotEnum.Weapon);
            Boostrap.Instance.PlayerData.selectedCharacter.AddItemToInventory(sword);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
}
