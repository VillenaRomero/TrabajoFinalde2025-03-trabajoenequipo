using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playermoved : MonoBehaviour
{
    [Header("Cámaras de seguridad")]
    public CinemachineCamera[] securityCameras;
    private int currentCameraIndex = 0;

    [Header("Jugador")]
    public CinemachineCamera playerCamera;
    private bool inSecurityMode = false;

    [Header("UI Seguridad")]
    public GameObject uiCanvas;
    public Image blackScreen;
    public Text errorText;
    public Image mapImage;

    [Header("Cámara - Tiempo límite")]
    public float cameraActiveTime = 30f;
    private float cameraTimer = 0f;
    private bool cameraError = false;

    void Start()
    {
        EnablePlayerCamera();

        if (blackScreen) blackScreen.enabled = false;
        if (errorText) errorText.enabled = false;
        if (mapImage) mapImage.enabled = false;
        if (uiCanvas) uiCanvas.SetActive(false);
    }

    void Update()
    {
        // --- ENTRAR / SALIR con TAB ---
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inSecurityMode)
                EnterSecurityMode();
            else
                ExitSecurityMode();
        }

        if (!inSecurityMode) return;

        // --- CAMBIO DIRECTO ENTRE CÁMARAS ---
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchToPreviousCamera();
        if (Input.GetKeyDown(KeyCode.E))
            SwitchToNextCamera();

        // --- TIMER DE ERROR ---
        if (!cameraError)
        {
            cameraTimer -= Time.deltaTime;
            if (cameraTimer <= 0f)
                CameraError();
        }
    }

    // --- ENTRADA / SALIDA ---

    void EnterSecurityMode()
    {
        inSecurityMode = true;
        playerCamera.Priority = 0;

        currentCameraIndex = 0;
        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();

        if (uiCanvas) uiCanvas.SetActive(true);
        if (mapImage) mapImage.enabled = true;

        ForceInstantBlend();
        Debug.Log("Entrando al modo de cámaras de seguridad.");
    }

    void ExitSecurityMode()
    {
        inSecurityMode = false;
        DisableAllCinemachineCameras();
        EnablePlayerCamera();

        if (uiCanvas) uiCanvas.SetActive(false);
        if (mapImage) mapImage.enabled = false;

        ForceInstantBlend();
        Debug.Log("Saliendo del modo de cámaras de seguridad.");
    }

    // --- CAMBIO DE CÁMARAS ---

    void SwitchToNextCamera()
    {
        if (cameraError) return;

        securityCameras[currentCameraIndex].Priority = 0;
        currentCameraIndex = (currentCameraIndex + 1) % securityCameras.Length;

        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();
        ForceInstantBlend();

        Debug.Log("Cambiando a cámara " + (currentCameraIndex + 1));
    }

    void SwitchToPreviousCamera()
    {
        if (cameraError) return;

        securityCameras[currentCameraIndex].Priority = 0;
        currentCameraIndex--;
        if (currentCameraIndex < 0)
            currentCameraIndex = securityCameras.Length - 1;

        EnableOnlyCamera(securityCameras[currentCameraIndex]);
        ResetCameraTimer();
        ForceInstantBlend();

        Debug.Log("Cambiando a cámara " + (currentCameraIndex + 1));
    }

    // --- CONTROL DE CÁMARAS ---

    void EnableOnlyCamera(CinemachineCamera cam)
    {
        DisableAllCinemachineCameras();
        if (cam != null)
            cam.Priority = 20;
    }

    void DisableAllCinemachineCameras()
    {
        for (int i = 0; i < securityCameras.Length; i++)
        {
            if (securityCameras[i])
                securityCameras[i].Priority = 0;
        }
    }

    void EnablePlayerCamera()
    {
        playerCamera.Priority = 30;
    }

    // --- ERRORES Y TIMER ---

    void ResetCameraTimer()
    {
        cameraTimer = cameraActiveTime;
        cameraError = false;
        if (blackScreen) blackScreen.enabled = false;
        if (errorText) errorText.enabled = false;
    }

    void CameraError()
    {
        cameraError = true;
        if (blackScreen) blackScreen.enabled = true;
        if (errorText)
        {
            errorText.enabled = true;
            errorText.text = "Error de cámara";
        }
        Debug.Log("La cámara ha fallado por tiempo.");
    }

    // --- CAMBIO INSTANTÁNEO (SIMPLE) ---
    void ForceInstantBlend()
    {
        var mainCam = Camera.main;
        if (mainCam == null) return;

        var brain = mainCam.GetComponent<CinemachineBrain>();
        if (brain == null) return;

        // Método más simple y seguro: forzar refresco inmediato
        brain.enabled = false;
        brain.enabled = true;
    }
}
