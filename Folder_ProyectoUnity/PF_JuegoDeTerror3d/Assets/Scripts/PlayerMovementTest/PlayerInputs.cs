using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private FirstPersonCameraController fpccNormal;
    [SerializeField] private FirstPersonCameraController fpccCrouch;
    private PlayerController _pc;
    private GameInputs _gameInputs;

    private void Awake()
    {
        _gameInputs = new GameInputs();
        _pc = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        _gameInputs.Enable();
        RegisterAction(_gameInputs.Player.Movement, OnMovement);
        RegisterAction(_gameInputs.Player.Run, OnCharacterRun);
        RegisterAction(_gameInputs.Player.Crouch, OnCharacterCrouch);
        RegisterAction(_gameInputs.Player.CameraLook, OnCameraLook);
        PlayerController.OnCrouchZoneExit += OnCrouchZoneExit;
    }

    private void OnDisable()
    {
        _gameInputs.Disable();
        UnregisterAction(_gameInputs.Player.Movement, OnMovement);
        UnregisterAction(_gameInputs.Player.Run, OnCharacterRun);
        UnregisterAction(_gameInputs.Player.Crouch, OnCharacterCrouch);
        UnregisterAction(_gameInputs.Player.CameraLook, OnCameraLook);
        PlayerController.OnCrouchZoneExit -= OnCrouchZoneExit;
    }

    private void RegisterAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    private void UnregisterAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _pc.SetMovementInput(context.ReadValue<Vector2>());
    }
    private void OnCharacterRun(InputAction.CallbackContext context)
    {
        UpdateMovementState();
    }
    private void OnCharacterCrouch(InputAction.CallbackContext context)
    {
        UpdateMovementState();
    }
    private void OnCameraLook(InputAction.CallbackContext context)
    {
        fpccNormal.SetLookInput(context.ReadValue<Vector2>());
        fpccCrouch.SetLookInput(context.ReadValue<Vector2>());
    }

    private void UpdateMovementState()
    {
        if (_pc.IsInCrouchZone)
        {
            _pc.UpdateMovementState(MovementState.Crouching);
            return;
        }

        if (_gameInputs.Player.Run.IsPressed())
        {
            _pc.UpdateMovementState(MovementState.Running);
        }
        else if (_gameInputs.Player.Crouch.IsPressed())
        {
            _pc.UpdateMovementState(MovementState.Crouching);
        }
        else if (_pc.MovementInput.x != 0 || _pc.MovementInput.y != 0)
        {
            _pc.UpdateMovementState(MovementState.Walking);
        }
        else
        {
            _pc.UpdateMovementState(MovementState.Idle);
        }
    }

    private void OnCrouchZoneExit()
    {
        if (!_gameInputs.Player.Crouch.IsPressed())
        {
            if (_gameInputs.Player.Run.IsPressed())
            {
                _pc.UpdateMovementState(MovementState.Running);
            }
            else if(_pc.MovementInput.x != 0 || _pc.MovementInput.y != 0)
            {
                _pc.UpdateMovementState(MovementState.Walking);
            }
            else
            {
                _pc.UpdateMovementState(MovementState.Idle);
            }
        }
    }
}
