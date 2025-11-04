using UnityEngine;

public class WinZone : MonoBehaviour//->Mochar
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneController.Instance.WinLevel();
        }
    }
}
