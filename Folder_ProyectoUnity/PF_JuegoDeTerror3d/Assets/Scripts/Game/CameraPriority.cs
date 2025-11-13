using Unity.Cinemachine;
using UnityEngine;

public class Camarapriiority : MonoBehaviour //->Refactorizalo con colas y el sistema debe estar en game maanger
{
    public CinemachineCamera currentCamera;
    void Start()
    {
        currentCamera.Priority++;
    }
    public void UpdateCamera(CinemachineCamera target)
    {
        currentCamera.Priority--;
        currentCamera = target;
        currentCamera.Priority++;
        currentCamera.Priority++;
    }
}
