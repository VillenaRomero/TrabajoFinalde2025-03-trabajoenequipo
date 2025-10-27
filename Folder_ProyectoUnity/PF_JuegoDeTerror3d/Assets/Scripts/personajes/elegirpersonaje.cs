using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class elegirpersonaje : MonoBehaviour
{
    [Header("Personajes (asigna en orden)")]
    [SerializeField] private Transform[] personajes;

    [Header("Posiciones de referencia (5 puntos en la escena)")]
    [SerializeField] private Transform[] posiciones;

    [Header("UI")]
    [SerializeField] private TMP_Text textoSeleccion;

    [Header("Velocidad de movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;

    private int personajeCentral = 1;

    void Start()
    {
        ActualizarPosiciones(true);
        ActualizarTexto();
    }

    void Update()
    {
        for (int i = 0; i < personajes.Length; i++)
        {
            if (i < posiciones.Length)
            {
                personajes[i].position = Vector3.Lerp(
                    personajes[i].position,
                    posiciones[i].position,
                    Time.deltaTime * velocidadMovimiento
                );

                personajes[i].rotation = Quaternion.Lerp(
                    personajes[i].rotation,
                    posiciones[i].rotation,
                    Time.deltaTime * velocidadMovimiento
                );
            }
        }
    }

    // 🔹 Actualiza posiciones iniciales o instantáneas
    public void ActualizarPosiciones(bool instantaneo = false)
    {
        for (int i = 0; i < personajes.Length; i++)
        {
            if (i < posiciones.Length)
            {
                if (instantaneo)
                {
                    personajes[i].position = posiciones[i].position;
                    personajes[i].rotation = posiciones[i].rotation;
                }
            }
        }
    }

    // 🔹 Botón izquierda
    public void MoverIzquierda()
    {
        Transform temp = personajes[0];
        personajes[0] = personajes[1];
        personajes[1] = personajes[2];
        personajes[2] = temp;

        ActualizarPosiciones();
        ActualizarTexto();
    }

    // 🔹 Botón derecha
    public void MoverDerecha()
    {
        Transform temp = personajes[2];
        personajes[2] = personajes[1];
        personajes[1] = personajes[0];
        personajes[0] = temp;

        ActualizarPosiciones();
        ActualizarTexto();
    }

    // 🔹 Botón seleccionar
    public void SeleccionarPersonaje()
    {
        Transform seleccionado = personajes[1]; // El del centro
        Debug.Log(" Seleccionaste a: " + seleccionado.name);

        if (textoSeleccion != null)
            textoSeleccion.text = "Seleccionado: " + seleccionado.name;

        // Guardar selección global (si tienes un GameManager)
        if (GameManager.instance != null)
            GameManager.instance.SetPersonaje(seleccionado);
    }

    // 🔹 Actualiza el texto del personaje actual
    public void ActualizarTexto()
    {
        if (textoSeleccion != null)
            textoSeleccion.text = "Eligiendo: " + personajes[1].name;
    }
}
