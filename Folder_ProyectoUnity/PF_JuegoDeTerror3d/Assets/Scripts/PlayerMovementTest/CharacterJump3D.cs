using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterJump3D : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private bool enableJump = true;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Detection Settings")]
    [SerializeField] private Vector3 boxCastSize = new Vector3(0.8f, 0.1f, 0.8f);
    [SerializeField] private Vector3 boxCastOffset = new Vector3(0f, -0.5f, 0f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Multiple Jump Settings")]
    [SerializeField] private bool multipleJumps = false;
    [SerializeField] private int extraJumps = 1;

    private Rigidbody _playerRB;
    private int _currentJumpCount = 0;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && multipleJumps)
        {
            _currentJumpCount = extraJumps;
        }
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + boxCastOffset;
        bool hit = Physics.BoxCast(origin, boxCastSize * 0.5f, Vector3.down, Quaternion.identity, 0.05f, groundLayer);
        return hit;
    }

    public void OnCharacterJump(InputAction.CallbackContext context)
    {
        if (!enableJump) return;

        if (context.performed)
        {
            if (IsGrounded() || (multipleJumps && _currentJumpCount > 0))
            {
                Vector3 velocity = _playerRB.linearVelocity;
                velocity.y = 0;
                _playerRB.linearVelocity = velocity;
                _playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (multipleJumps)
                {
                    _currentJumpCount--;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + boxCastOffset;
        Gizmos.color = IsGrounded() ? Color.red : Color.green;
        Gizmos.DrawWireCube(origin, boxCastSize);
    }
}