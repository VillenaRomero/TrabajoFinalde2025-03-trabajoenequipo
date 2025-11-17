using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class CameraInfo {
    public Transform Position;
    public Transform Direction;
    public int Index;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform[] cameraPositions;
    [SerializeField] private Transform[] cameraDirections;
    [SerializeField] private Transform playerView;

    [SerializeField] private GameObject fnafUIPanel;
    [SerializeField] private TMP_Text cameraNameText;

    private bool isFnafMode = false;

    private DoubleList<CameraInfo> cameraList = new DoubleList<CameraInfo>();
    private Node<CameraInfo> currentNode;

    private int currentIndex = 0;

    void Start()
    {
        SetupCameras();
    }

    private void SetupCameras()
    {
        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            Debug.LogError("No hay posiciones de cámara.");
            return;
        }

        if (cameraDirections == null || cameraDirections.Length != cameraPositions.Length)
        {
            Debug.LogError("Las direcciones NO coinciden en cantidad con las posiciones.");
            return;
        }

        cameraList = new DoubleList<CameraInfo>();

        for (int i = 0; i < cameraPositions.Length; i++)
        {
            CameraInfo cameraInfo = new CameraInfo();
            cameraInfo.Position = cameraPositions[i];
            cameraInfo.Direction = cameraDirections[i];
            cameraInfo.Index = i;

            cameraList.AddNode(cameraInfo);
        }

        currentNode = cameraList.Head;

        if (fnafUIPanel != null)
            fnafUIPanel.SetActive(false);

        UpdateCameraText();
    }

    public void GoNext()
    {
        if (currentNode == null)
            currentNode = cameraList.Head;

        else if (currentNode.Next != null)
            currentNode = currentNode.Next;

        ApplyCamera();
    }

    public void GoPrev()
    {
        if (currentNode == null)
            currentNode = cameraList.Head;

        else if (currentNode.Prev != null)
            currentNode = currentNode.Prev;

        ApplyCamera();
    }

    public void GotoIndex(int index)
    {
        Node<CameraInfo> temp = cameraList.Head;

        while (temp != null)
        {
            if (temp.Value.Index == index)
            {
                currentNode = temp;
                ApplyCamera();
                return;
            }
            temp = temp.Next;
        }

        Debug.Log("Índice fuera de rango: " + index);
    }

    private void ApplyCamera()
    {
        if (mainCamera == null || currentNode == null)
            return;

        CameraInfo cam = currentNode.Value;

        if (cam.Position != null)
            mainCamera.transform.position = cam.Position.position;

        if (cam.Direction != null)
            mainCamera.transform.LookAt(cam.Direction);

        currentIndex = cam.Index;
        UpdateCameraText();
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isFnafMode = !isFnafMode;

        if (isFnafMode)
        {
            if (fnafUIPanel != null) fnafUIPanel.SetActive(true);
            ApplyCamera();
        }
        else
        {
            if (fnafUIPanel != null) fnafUIPanel.SetActive(false);
            ReturnToPlayerView();
        }
    }

    public void OnNextCamera(InputAction.CallbackContext context)
    {
        if (context.performed && isFnafMode)
            GoNext();
    }

    public void OnPreviousCamera(InputAction.CallbackContext context)
    {
        if (context.performed && isFnafMode)
            GoPrev();
    }

    private void ReturnToPlayerView()
    {
        if (mainCamera == null || playerView == null) return;

        mainCamera.transform.position = playerView.position;
        mainCamera.transform.rotation = playerView.rotation;
    }

    private void UpdateCameraText()
    {
        if (cameraNameText == null) return;
        cameraNameText.text = "Cámara " + (currentIndex + 1) + "/" + cameraPositions.Length;
    }
}
