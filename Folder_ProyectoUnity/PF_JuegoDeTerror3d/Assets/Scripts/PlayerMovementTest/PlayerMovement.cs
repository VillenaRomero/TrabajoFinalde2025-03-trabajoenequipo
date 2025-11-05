using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _pc;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (_pc.enableMovement)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector3 forward = new Vector3(_pc.cameraTransform.forward.x, 0f, _pc.cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(_pc.cameraTransform.right.x, 0f, _pc.cameraTransform.right.z).normalized;

        Vector3 moveDirection = (forward * _pc._inputZ + right * _pc._inputX).normalized;
        Vector3 velocity = moveDirection * _pc._actualSpeed;

        _pc._playerRB.linearVelocity = new Vector3(velocity.x, _pc._playerRB.linearVelocity.y, velocity.z);
    }
}