using UnityEngine;

public class DatosJugador : MonoBehaviour//->vuelan
{
    [Header("Modelo del personaje activo")]
    public GameObject modeloActual;

    public void AsignarPersonaje(GameObject prefabSeleccionado)
    {
        if (modeloActual != null)
            Destroy(modeloActual);

        modeloActual = Instantiate(prefabSeleccionado, transform);

        modeloActual.transform.localPosition = Vector3.zero;
        modeloActual.transform.localRotation = Quaternion.identity;
    }
}
