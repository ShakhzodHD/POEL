using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputOverlay : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private InputAction inventory;
    private InputAction leftMouseClick;

    private void Awake()
    {
        inventory = playerInput.actions.FindAction(InputActions.Inventory.ToString());
        leftMouseClick = playerInput.actions.FindAction(InputActions.LeftMouseClick.ToString());
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.Enable();
            inventory.performed += OnUseInventory;
        }

        if (leftMouseClick != null)
        {
            leftMouseClick.Enable();
            leftMouseClick.performed += OnMouseClick;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.performed -= OnUseInventory;
            inventory.Disable();
        }
        if (leftMouseClick != null)
        {
            leftMouseClick.performed -= OnMouseClick;
            leftMouseClick.Disable();
        }
    }
    private void OnUseInventory(InputAction.CallbackContext context)
    {
        Boostrap.Instance.UIManager.ToggleInventory();
    }

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        Boostrap.Instance.CentralizedClickHandler.HandleClick();
    }
}
