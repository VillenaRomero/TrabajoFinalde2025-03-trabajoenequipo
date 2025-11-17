using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float gamepadSensitivity = 80f;
    [SerializeField] private Transform playerBody;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera normalCinemachineCamera;
    [SerializeField] private CinemachineCamera crouchCinemachineCamera;

    private float xRotation = 0f;

    private Vector2 _lookInput;
    public Vector2 LookInput => _lookInput;
    public void SetLookInput(Vector2 input) => _lookInput = input;

    private void OnEnable()
    {
        PlayerController.OnMovementStateChange += UpdateCameraByState;
    }
    private void OnDisable()
    {
        PlayerController.OnMovementStateChange -= UpdateCameraByState;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        float sensitivity = Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero
            ? mouseSensitivity : gamepadSensitivity * Time.deltaTime;

        float mouseX = _lookInput.x * sensitivity;
        float mouseY = _lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void UpdateCameraByState(MovementState state)
    {
        if (normalCinemachineCamera == null || crouchCinemachineCamera == null)
            return;

        if (state == MovementState.Crouching)
        {
            crouchCinemachineCamera.Priority = 10;
            normalCinemachineCamera.Priority = 0;
        }
        else
        {
            crouchCinemachineCamera.Priority = 0;
            normalCinemachineCamera.Priority = 10;
        }
    }
}