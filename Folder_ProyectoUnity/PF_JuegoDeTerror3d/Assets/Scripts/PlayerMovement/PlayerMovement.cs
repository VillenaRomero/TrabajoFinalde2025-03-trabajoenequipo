using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController _pc;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (_pc.EnableMovement)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 forward = new Vector3(_pc.CameraTransform.forward.x, 0f, _pc.CameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(_pc.CameraTransform.right.x, 0f, _pc.CameraTransform.right.z).normalized;

        Vector3 moveDirection = (forward * _pc.MovementInput.y + right * _pc.MovementInput.x).normalized;
        Vector3 velocity = moveDirection * _pc.ActualSpeed;

        _pc.PlayerRB.linearVelocity = new Vector3(velocity.x, _pc.PlayerRB.linearVelocity.y, velocity.z);
    }
}