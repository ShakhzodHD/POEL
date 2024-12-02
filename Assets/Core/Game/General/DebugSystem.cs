using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private ItemDefinition weapon;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Boostrap.Instance.PlayerData.selectedCharacter.InventoryManager.TryAdd(weapon.CreateInstance());
        }
    }
}
