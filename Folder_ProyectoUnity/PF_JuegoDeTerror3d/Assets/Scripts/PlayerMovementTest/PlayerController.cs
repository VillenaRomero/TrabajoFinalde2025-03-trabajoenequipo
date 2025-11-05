using System;
using UnityEngine;

public enum MovementState
{
    Idle,
    Walking,
    Running,
    Crouching
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [field: Header("~ Movement Settings")]
    [field: SerializeField] public bool enableMovement { get; private set; } = true;
    [field: SerializeField] public float normalSpeed { get; private set; } = 5f;

    [field: Header("~ Run Settings")]
    [field: SerializeField] public bool enableRun { get; private set; } = true;
    [field: SerializeField] public float runSpeed { get; private set; } = 10f;

    [field: Header("~ Crouch Settings")]
    [field: SerializeField] public bool enableCrouch { get; private set; } = true;
    [field: SerializeField] public float crouchSpeed { get; private set; } = 2.5f;

    [field: Header("~ References")]
    [field: SerializeField] public Transform cameraTransform { get; private set; }

    public Rigidbody _playerRB { get; private set; }
    public float _actualSpeed { get; private set; }
    public float _inputX { get; private set; }
    public float _inputZ { get; private set; }
    public MovementState _currentMovementState { get; private set; } = MovementState.Idle;

    public static event Action<MovementState> OnMovementChange;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _actualSpeed = normalSpeed;
    }

    public void SetInputX(float inputX)
    {
        _inputX = inputX;
    }

    public void SetInputZ(float inputZ)
    {
        _inputZ = inputZ;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        _actualSpeed = newSpeed;
    }

    public void SetMovementState(MovementState newState)
    {
        if (_currentMovementState != newState)
        {
            _currentMovementState = newState;
            OnMovementChange?.Invoke(_currentMovementState);
        }
    }
}
