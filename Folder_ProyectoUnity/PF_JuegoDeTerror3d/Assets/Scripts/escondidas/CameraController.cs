using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CameraController : MonoBehaviour
{
    [Header("Cámara principal (única del juego)")]
    public Camera mainCamera;

    [Header("Posiciones FNAF (dónde estará la cámara)")]
    public Transform[] cameraPositions;

    [Header("Direcciones FNAF (hacia dónde mira la cámara)")]
    public Transform[] cameraDirections;

    [Header("Vista del jugador normal")]
    public Transform playerView;

    [Header("UI del modo FNAF")]
    public GameObject fnafUIPanel;
    public TMP_Text cameraNameText;

    private bool isFnafMode = false;

    private DoubleList<Transform> positionList = new DoubleList<Transform>();
    private DoubleList<Transform> directionList = new DoubleList<Transform>();

    private Node<Transform> currentPosNode;
    private Node<Transform> currentDirNode;
    private int currentIndex = 0;

    void Start()
    {
        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            Debug.LogError(" No hay posiciones de cámara asignadas.");
            return;
        }

        for (int i = 0; i < cameraPositions.Length; i++)
        {
            positionList.AddNode(cameraPositions[i]);
            if (cameraDirections != null && i < cameraDirections.Length)
                directionList.AddNode(cameraDirections[i]);
            else
                directionList.AddNode(null);
        }

        currentPosNode = positionList.Head;
        currentDirNode = directionList.Head;

        if (fnafUIPanel != null)
            fnafUIPanel.SetActive(false);

        UpdateCameraText();
    }

    public void GoToCamera(int index)
    {
        if (mainCamera == null)
        {
            Debug.LogError(" MainCamera no asignada.");
            return;
        }

        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            Debug.LogError(" No hay posiciones de cámara asignadas.");
            return;
        }

        if (index < 0 || index >= cameraPositions.Length)
        {
            Debug.LogWarning(" Índice fuera de rango: " + index);
            return;
        }

        mainCamera.transform.position = cameraPositions[index].position;

        if (cameraDirections != null && index < cameraDirections.Length && cameraDirections[index] != null)
            mainCamera.transform.LookAt(cameraDirections[index]);
        else
            Debug.LogWarning(" Dirección de cámara no asignada en el índice " + index);

        currentPosNode = positionList.Head;
        currentDirNode = directionList.Head;
        for (int i = 0; i < index && currentPosNode.Next != null; i++)
        {
            currentPosNode = currentPosNode.Next;
            currentDirNode = currentDirNode.Next;
        }

        currentIndex = index;
        UpdateCameraText();
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isFnafMode = !isFnafMode;

        if (isFnafMode)
        {
            if (fnafUIPanel != null)
                fnafUIPanel.SetActive(true);

            UpdateCameraTransform();
        }
        else
        {
            if (fnafUIPanel != null)
                fnafUIPanel.SetActive(false);

            VolverAVistaJugador();
        }
    }

    public void OnNextCamera(InputAction.CallbackContext context)
    {
        if (!context.performed || !isFnafMode) return;
        GoNextCamera();
    }

    public void OnPreviousCamera(InputAction.CallbackContext context)
    {
        if (!context.performed || !isFnafMode) return;
        GoPreviousCamera();
    }

    private void GoNextCamera()
    {
        if (currentPosNode == null || currentPosNode.Next == null)
        {
            Debug.Log(" No hay siguiente cámara.");
            return;
        }

        currentPosNode = currentPosNode.Next;
        currentDirNode = currentDirNode.Next;
        currentIndex++;

        UpdateCameraTransform();
        UpdateCameraText();
    }

    private void GoPreviousCamera()
    {
        if (currentPosNode == null || currentPosNode.Prev == null)
        {
            Debug.Log(" No hay cámara anterior.");
            return;
        }

        currentPosNode = currentPosNode.Prev;
        currentDirNode = currentDirNode.Prev;
        currentIndex--;

        UpdateCameraTransform();
        UpdateCameraText();
    }

    private void UpdateCameraTransform()
    {
        if (mainCamera == null || currentPosNode == null) return;

        mainCamera.transform.position = currentPosNode.Value.position;

        if (currentDirNode != null && currentDirNode.Value != null)
            mainCamera.transform.LookAt(currentDirNode.Value);
    }

    private void VolverAVistaJugador()
    {
        if (mainCamera != null && playerView != null)
        {
            mainCamera.transform.position = playerView.position;
            mainCamera.transform.rotation = playerView.rotation;
        }
    }

    private void UpdateCameraText()
    {
        if (cameraNameText == null) return;
        cameraNameText.text = "Cámara " + (currentIndex + 1) + "/" + cameraPositions.Length;
    }
}
