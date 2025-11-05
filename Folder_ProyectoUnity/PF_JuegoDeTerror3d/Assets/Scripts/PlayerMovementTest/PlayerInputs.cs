using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController _pc;

    [SerializeField] private InputActionAsset inputActions;
    private InputAction runAction;
    private InputAction crouchAction;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
        runAction = inputActions.FindAction("Run", true);
        crouchAction = inputActions.FindAction("Crouch", true);
    }

    private void OnEnable()
    {
        runAction?.Enable();
        crouchAction?.Enable();
    }

    private void OnDisable()
    {
        runAction?.Disable();
        crouchAction?.Disable();
    }

    public void OnXMovement(InputAction.CallbackContext context)
    {
        _pc.SetInputX(context.ReadValue<float>());
        UpdateMovementState();
    }

    public void OnZMovement(InputAction.CallbackContext context)
    {
        _pc.SetInputZ(context.ReadValue<float>());
        UpdateMovementState();
    }

    public void OnCharacterRun(InputAction.CallbackContext context)
    {
        UpdateMovementState();
    }

    public void OnCharacterCrouch(InputAction.CallbackContext context)
    {
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        if (runAction != null && runAction.IsPressed())
        {
            _pc.SetMovementState(MovementState.Running);
        }
        else if (crouchAction != null && crouchAction.IsPressed())
        {
            _pc.SetMovementState(MovementState.Crouching);
        }
        else if (_pc._inputX != 0 || _pc._inputZ != 0)
        {
            _pc.SetMovementState(MovementState.Walking);
        }
        else
        {
            _pc.SetMovementState(MovementState.Idle);
        }
    }
}
