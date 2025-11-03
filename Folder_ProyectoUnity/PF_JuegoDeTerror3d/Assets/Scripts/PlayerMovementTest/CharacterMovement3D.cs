using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovementSettings
{

}

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool enableMovement = true;
    [field: SerializeField] public float moveSpeed { get; private set; } = 5f;
    

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody _playerRB;
    private float _horizontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (enableMovement)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * _verticalInput + right * _horizontalInput).normalized;
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = _playerRB.linearVelocity.y;
        _playerRB.linearVelocity = velocity;
    }

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<float>();
    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        _verticalInput = context.ReadValue<float>();
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}