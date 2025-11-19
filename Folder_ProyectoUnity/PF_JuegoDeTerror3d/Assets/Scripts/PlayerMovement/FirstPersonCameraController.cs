using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

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

    [SerializeField] private CinemachineBasicMultiChannelPerlin normalMultiChannelPerlin;
    [SerializeField] private CinemachineBasicMultiChannelPerlin crouchMultiChannelPerlin;

    [SerializeField] private PlayerController _pc;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    public float MouseSensitivity => mouseSensitivity;
    public float GamepadSensitivity => gamepadSensitivity;

    private Coroutine currentShakeRoutine;

    private void OnEnable()
    {
        PlayerController.OnMovementStateChange += UpdateCameraByState;
        PlayerController.OnMovementStateChange += UpdateCameraShakeByState;
    }

    private void OnDisable()
    {
        PlayerController.OnMovementStateChange -= UpdateCameraByState;
        PlayerController.OnMovementStateChange -= UpdateCameraShakeByState;
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

    private void UpdateCameraShakeByState(MovementState state)
    {
        if (currentShakeRoutine != null)
        {
            StopCoroutine(currentShakeRoutine);
        }

        float targetAmpGain;
        float targetFreqGain;

        if (!_pc.IsMoving)
        {
            targetAmpGain = 0.1f;
            targetFreqGain = 1f;
        }
        else
        {
            if (state == MovementState.Walking)
            {
                targetAmpGain = 0.4f;
                targetFreqGain = 2f;
            }
            else if (state == MovementState.Crouching)
            {
                targetAmpGain = 0.2f;
                targetFreqGain = 2f;
            }
            else if (state == MovementState.Running)
            {
                targetAmpGain = 0.5f;
                targetFreqGain = 4f;
            }
            else
            {
                return;
            }
        }
        currentShakeRoutine = StartCoroutine(FadeCameraShakeRoutine(normalMultiChannelPerlin, 
            crouchMultiChannelPerlin,targetAmpGain, targetFreqGain, fadeDuration));
    }

    private IEnumerator FadeCameraShakeRoutine(CinemachineBasicMultiChannelPerlin cbmcp1, 
        CinemachineBasicMultiChannelPerlin cbmcp2, float targetAmpGain, float targetFreqGain, float duration)
    {
        float startAmp1 = cbmcp1.AmplitudeGain;
        float startFreq1 = cbmcp1.FrequencyGain;
        float startAmp2 = cbmcp2.AmplitudeGain;
        float startFreq2 = cbmcp2.FrequencyGain;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = Mathf.SmoothStep(0.0f, 1.0f, t);

            cbmcp1.AmplitudeGain = Mathf.Lerp(startAmp1, targetAmpGain, t);
            cbmcp1.FrequencyGain = Mathf.Lerp(startFreq1, targetFreqGain, t);

            cbmcp2.AmplitudeGain = Mathf.Lerp(startAmp2, targetAmpGain, t);
            cbmcp2.FrequencyGain = Mathf.Lerp(startFreq2, targetFreqGain, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cbmcp1.AmplitudeGain = targetAmpGain;
        cbmcp1.FrequencyGain = targetFreqGain;
        cbmcp2.AmplitudeGain = targetAmpGain;
        cbmcp2.FrequencyGain = targetFreqGain;
    }
}
