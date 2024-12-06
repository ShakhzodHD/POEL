using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private ItemDefinition weapon;
    [SerializeField] private ItemDefinition helmert;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Boostrap.Instance.PlayerData.selectedCharacter.MainInventoryManager.TryAdd(weapon.CreateInstance());
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            Boostrap.Instance.PlayerData.selectedCharacter.MainInventoryManager.TryAdd(helmert.CreateInstance());
        }
    }
}
