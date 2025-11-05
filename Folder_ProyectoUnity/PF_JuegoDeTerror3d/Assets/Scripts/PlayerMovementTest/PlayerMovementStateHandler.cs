using UnityEngine;

public class PlayerMovementStateHandler : MonoBehaviour
{
    private PlayerController _pc;

    private void OnEnable()
    {
        PlayerController.OnMovementChange += OnMovementStateChanged;
    }
    private void OnDisable()
    {
        PlayerController.OnMovementChange -= OnMovementStateChanged;
    }

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }

    private void OnMovementStateChanged(MovementState state)
    {
        if (state == MovementState.Running && _pc.enableRun)
        {
            _pc.SetMoveSpeed(_pc.runSpeed);
        }
        else if (state == MovementState.Crouching && _pc.enableCrouch)
        {
            _pc.SetMoveSpeed(_pc.crouchSpeed); 
        }
        else
        {
            _pc.SetMoveSpeed(_pc.normalSpeed);
        }
    }
}