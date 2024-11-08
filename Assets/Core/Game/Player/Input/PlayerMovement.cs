using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Camera gameCamera;

    [Header("Settings")]
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private InputAction moveAction;
    private bool isMobile;

    private void Awake()
    {
        isMobile = Application.isMobilePlatform;

        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (gameCamera == null) gameCamera = FindObjectOfType<Camera>(); // fix

        agent.stoppingDistance = stoppingDistance;

        if (isMobile)
        {
            EnhancedTouchSupport.Enable();
        }

        moveAction = playerInput.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        if (moveAction != null && !isMobile)
        {
            moveAction.Enable();
            moveAction.performed += HandleMove;
        }
    }

    private void OnDisable()
    {
        if (moveAction != null && !isMobile)
        {
            moveAction.performed -= HandleMove;
            moveAction.Disable();
        }
    }

    private void Update()
    {
        if (isMobile)
        {
            HandleMobileInput();
        }
    }

    private void HandleMobileInput()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                MoveToPosition(touch.screenPosition);
            }
        }
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        if (!isMobile)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            MoveToPosition(mousePosition);
        }
    }

    private void MoveToPosition(Vector2 screenPosition)
    {
        if (!enabled || gameCamera == null) return;

        Ray ray = gameCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }

    public void StopMovement()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.ResetPath();
        }
    }

    private void OnDestroy()
    {
        if (isMobile && EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Disable();
        }
    }
}