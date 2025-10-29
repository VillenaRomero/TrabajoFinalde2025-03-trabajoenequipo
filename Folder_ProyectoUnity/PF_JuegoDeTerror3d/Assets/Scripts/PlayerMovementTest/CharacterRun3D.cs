using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement3D))]
public class CharacterRun3D : MonoBehaviour
{
    [Header("Run Settings")]
    [SerializeField] private bool enableRun = true;
    [SerializeField] private float runSpeed = 10f;

    private CharacterMovement3D _characterMovement;
    private float _originalMoveSpeed;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement3D>();
        _originalMoveSpeed = _characterMovement.moveSpeed;
    }

    public void OnCharacterRun(InputAction.CallbackContext context)
    {
        if (!enableRun)
        {
            _characterMovement.SetMoveSpeed(_originalMoveSpeed);
            return;
        }

        if (context.performed)
        {
            _characterMovement.SetMoveSpeed(runSpeed);
        }
        else if (context.canceled)
        {
            _characterMovement.SetMoveSpeed(_originalMoveSpeed);
        }
    }
}