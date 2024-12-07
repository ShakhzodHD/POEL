using UnityEngine;
using TMPro;

public class CentralizedClickHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private void Start()
    {
        if (playerCamera == null) playerCamera = Boostrap.Instance.Camera;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
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
                var itemDifinition = hit.collider.GetComponentInParent<ItemData>();

                Boostrap.Instance.PlayerData.selectedCharacter.MainInventoryManager.TryAdd(itemDifinition.ItemDefinition);

                Destroy(itemDifinition.gameObject);
            }
        }
    }
}
