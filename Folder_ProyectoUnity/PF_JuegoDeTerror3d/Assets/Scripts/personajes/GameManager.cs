using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Guarda el personaje seleccionado
    public Transform personajeSeleccionado;

    void Awake()
    {
        // Patrón Singleton: se mantiene entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para guardar el personaje elegido
    public void SetPersonaje(Transform personaje)
    {
        personajeSeleccionado = personaje;
        Debug.Log("🧩 Personaje guardado: " + personaje.name);
    }
}
