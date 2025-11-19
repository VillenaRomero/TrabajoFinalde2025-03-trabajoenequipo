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

    [SerializeField] private CinemachineInputAxisController normalInputAxisController;
    [SerializeField] private CinemachineInputAxisController crouchInputAxisController;

    public float MouseSensitivity => mouseSensitivity;
    public float GamepadSensitivity => gamepadSensitivity;

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
        SetSensitivity(mouseSensitivity, normalInputAxisController);
        SetSensitivity(mouseSensitivity, crouchInputAxisController);
    }

    public void SetSensitivity(float newSensitivity, CinemachineInputAxisController inputAxisController)
    {
        foreach (var controller in inputAxisController.Controllers)
        {
            if (controller.Name == "Look X (Pan)")
            {
                controller.Input.Gain = newSensitivity;
            }
            else if (controller.Name == "Look Y (Tilt)")
            {
                controller.Input.Gain = -newSensitivity;
            }
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