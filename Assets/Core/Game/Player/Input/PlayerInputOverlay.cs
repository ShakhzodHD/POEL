using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputOverlay : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private InputAction inventory;
    private void Awake()
    {
        inventory = playerInput.actions.FindAction(InputActions.Inventory.ToString());
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.Enable();
            inventory.performed += OnUseInventory;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.Enable();
            inventory.performed -= OnUseInventory;
        }
    }
    private void OnUseInventory(InputAction.CallbackContext context)
    {
        Boostrap.Instance.UIManager.ToggleInventory();
    }
}
