using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    private bool canTalk = false;
    private Objectgame npcObject;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canTalk && npcObject != null)
        {
            npcObject.MostrarDialogo();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            npcObject = collision.gameObject.GetComponent<Objectgame>();
            canTalk = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            npcObject.OcultarDialogo();
            npcObject = null;
            canTalk = false;
        }
    }
}
