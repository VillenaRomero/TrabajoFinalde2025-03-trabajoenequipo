using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Cámara principal (única del juego)")]
    public Camera mainCamera;

    [Header("Posiciones FNAF (dónde estará la cámara)")]
    public Transform[] cameraPositions;//-> lista doble enlazada

    [Header("Direcciones FNAF (hacia dónde mira la cámara)")]
    public Transform[] cameraDirections;

    [Header("Vista del jugador (posición y rotación normal)")]
    public Transform playerView;

    [Header("Panel de UI para cámaras FNAF")]
    public GameObject fnafUIPanel;

    private bool isFnafMode = false;

    void Start()
    {
        if (fnafUIPanel != null)
            fnafUIPanel.SetActive(false);
    }

    void Update()
    {
    }
    

    public void GoToCamera(int index)
    {
        if (mainCamera == null) return;
        if (index < 0 || index >= cameraPositions.Length) return;

        mainCamera.transform.position = cameraPositions[index].position;

        if (index < cameraDirections.Length && cameraDirections[index] != null)
        {
            mainCamera.transform.LookAt(cameraDirections[index]);
        }
    }

    private void VolverAVistaJugador()
    {
        if (mainCamera != null && playerView != null)
        {
            mainCamera.transform.position = playerView.position;
            mainCamera.transform.rotation = playerView.rotation;
        }
    }
    private void OnCamera(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isFnafMode = !isFnafMode;

        if (isFnafMode)
        {
            if (fnafUIPanel != null)
                fnafUIPanel.SetActive(true);
        }
        else
        {
            if (fnafUIPanel != null)
                fnafUIPanel.SetActive(false);
            VolverAVistaJugador();
        }
    }
}
