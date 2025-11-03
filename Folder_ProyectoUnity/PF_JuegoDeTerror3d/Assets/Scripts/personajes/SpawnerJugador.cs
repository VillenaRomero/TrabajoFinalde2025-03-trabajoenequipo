using UnityEngine;

public class SpawnerJugador : MonoBehaviour
{
    void Start()
    {
        if (GameManagerPersonaje.instance != null && GameManagerPersonaje.instance.personajeSeleccionadoPrefab != null)
        {
            // Buscar el Player en la escena
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Obtener el script que controlará los datos del personaje
                DatosJugador datos = player.GetComponent<DatosJugador>();

                if (datos != null)
                {
                    // Asignar el personaje seleccionado
                    datos.AsignarPersonaje(GameManagerPersonaje.instance.personajeSeleccionadoPrefab);
                    Debug.Log(" Personaje asignado al Player: " + GameManagerPersonaje.instance.personajeSeleccionadoPrefab.name);
                }
                
            }
            
        }
    }
}
