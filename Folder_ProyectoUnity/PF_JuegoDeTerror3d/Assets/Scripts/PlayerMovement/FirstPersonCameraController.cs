using Unity.Cinemachine;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float gamepadSensitivity = 200f;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera normalCinemachineCamera;
    [SerializeField] private CinemachineCamera crouchCinemachineCamera;

    public float CameraSensitivity => mouseSensitivity;

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