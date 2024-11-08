using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSkillsInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Player player;

    private InputAction major;
    private InputAction minor;
    private InputAction escape;
    private void Awake()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (player == null) player = GetComponent<Player>();

        major = playerInput.actions.FindAction("UseMajorAbility");
        minor = playerInput.actions.FindAction("UseMinorAbility");
        escape = playerInput.actions.FindAction("UseEscapeAbility");
    }
    private void OnEnable()
    {
        if (major != null && minor != null && escape != null) 
        {
            major.Enable();
            major.performed += OnUseMajorAbility;

            minor.Enable();
            minor.performed += OnUseMinorAbility;

            escape.Enable();
            escape.performed += OnUseEscapeAbility;
        }
    }

    private void OnDisable()
    {
        if (major != null && minor != null && escape != null)
        {
            major.performed -= OnUseMajorAbility;
            major.Disable();

            minor.performed -= OnUseMinorAbility;
            minor.Disable();

            escape.performed -= OnUseEscapeAbility;
            escape.Disable();
        }
    }

    public void OnUseMajorAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.UseAbility(AbilitySlotType.Major);
        }
    }

    public void OnUseMinorAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.UseAbility(AbilitySlotType.Minor);
        }
    }

    public void OnUseEscapeAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.UseAbility(AbilitySlotType.Escape);
        }
    }
}
