using UnityEngine;

public class GameManagerPersonaje : MonoBehaviour
{
    public static GameManagerPersonaje instance;

    [Header("Personaje Seleccionado")]
    public GameObject personajeSeleccionadoPrefab;

    private void Awake()
    {
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

    public void SetPersonaje(Transform personaje)
    {
        personajeSeleccionadoPrefab = personaje.gameObject;
    }

    public void ResetSeleccion()
    {
        personajeSeleccionadoPrefab = null;
        Destroy(gameObject); //-> No tiene sentido 
        instance = null;//-> Eliminado
    }
}
