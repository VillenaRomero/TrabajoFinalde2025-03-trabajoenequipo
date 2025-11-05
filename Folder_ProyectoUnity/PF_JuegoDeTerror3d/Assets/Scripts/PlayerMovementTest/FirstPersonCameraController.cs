using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float gamepadSensitivity = 80f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;
    private Vector2 _lookInput;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float sensitivity = Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero
            ? mouseSensitivity
            : gamepadSensitivity * Time.deltaTime;

        float mouseX = _lookInput.x * sensitivity;
        float mouseY = _lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}