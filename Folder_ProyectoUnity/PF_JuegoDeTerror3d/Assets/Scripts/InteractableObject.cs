using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour ,Iinteractable
{
    public UnityEvent OnInteract;
    public void Interact(GameObject player)
    {
        OnInteract.Invoke();
    }

}
