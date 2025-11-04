using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaCombate3D : MonoBehaviour
{
    public int cantidadCartas = 12;
    public GameObject cartaPrefab;
    public Transform zonaJugador, zonaEnemigo;
    public Material[] materialesCartas; // 52 materiales (una por carta)
    public Material reversoMaterial;

    public Text textoEstado;
    public Text textoResultado;
    public Text vidaJugadorText;
    public Text vidaEnemigoText;

    private Carts[] cartasJugador;
    private Carts[] cartasEnemigo;
    private Carts3D[] cartasObjJugador;
    private Carts3D[] cartasObjEnemigo;

    private int vidaJugador = 5;
    private int vidaEnemigo = 5;
    private int rondasCompletas = 0;
    private bool turnoJugador = true;

    private Carts cartaAtacante;
    private Carts cartaDefensora;
    private int indexAtacante;
    private int indexDefensor;

    void Start()
    {
        IniciarJuego();
    }

    void IniciarJuego()
    {
        cartasJugador = new Carts[cantidadCartas];
        cartasEnemigo = new Carts[cantidadCartas];
        cartasObjJugador = new Carts3D[cantidadCartas];
        cartasObjEnemigo = new Carts3D[cantidadCartas];

        Carts[] baraja = CrearBaraja();
        RepartirCartas(baraja);
        MostrarCartasOcultas();
        ActualizarUI();
        textoEstado.text = "Tu turno: Ataca";
    }

    Carts[] CrearBaraja()
    {
        Carts[] baraja = new Carts[52];
        int index = 0;
        for (int palo = 0; palo < 4; palo++)
        {
            for (int valor = 1; valor <= 13; valor++)
            {
                baraja[index] = new Carts((Palo)palo, valor, materialesCartas[index]);
                index++;
            }
        }

        for (int i = 0; i < 52; i++)
        {
            int r = Random.Range(0, 52);
            Carts temp = baraja[i];
            baraja[i] = baraja[r];
            baraja[r] = temp;
        }

        return baraja;
    }

    void RepartirCartas(Carts[] baraja)
    {
        for (int i = 0; i < cantidadCartas; i++)
        {
            cartasJugador[i] = baraja[i];
            cartasEnemigo[i] = baraja[i + cantidadCartas];
        }
    }

    void MostrarCartasOcultas()
    {
        for (int i = 0; i < cantidadCartas; i++)
        {
            cartasObjJugador[i] = CrearCarta(i, zonaJugador, cartasJugador[i], true);
            cartasObjEnemigo[i] = CrearCarta(i, zonaEnemigo, cartasEnemigo[i], false);
        }
    }

    Carts3D CrearCarta(int index, Transform zona, Carts datos, bool esJugador)
    {
        GameObject carta = Instantiate(cartaPrefab, zona);
        Carts3D c3d = carta.GetComponent<Carts3D>();
        c3d.ConfigurarCarta(datos, reversoMaterial);

        if (esJugador)
        {
            int idx = index;
            carta.AddComponent<BoxCollider>();
            carta.AddComponent<CartaSeleccion>().Configurar(this, idx);
        }
        return c3d;
    }

    public void AlSeleccionarCarta(int index)
    {
        if (turnoJugador)
        {
            cartaAtacante = cartasJugador[index];
            indexAtacante = index;
            cartasObjJugador[index].Revelar();

            indexDefensor = BuscarSiguienteCartaEnemigo();
            if (indexDefensor == -1) return;

            cartaDefensora = cartasEnemigo[indexDefensor];
            cartasObjEnemigo[indexDefensor].Revelar();

            ResolverTurno();
        }
        else
        {
            cartaDefensora = cartasJugador[index];
            indexDefensor = index;
            cartasObjJugador[index].Revelar();
            cartasObjEnemigo[indexAtacante].Revelar();
            ResolverTurno();
        }
    }

    int BuscarSiguienteCartaEnemigo()
    {
        for (int i = 0; i < cantidadCartas; i++)
        {
            if (cartasObjEnemigo[i] != null)
            {
                return i;
            }
        }
        return -1;
    }

    void ResolverTurno()
    {
        if (cartaAtacante.valor > cartaDefensora.valor)
        {
            if (turnoJugador)
            {
                vidaEnemigo--;
                textoResultado.text = "¡Ganaste la ronda!";
            }
            else
            {
                vidaJugador--;
                textoResultado.text = "¡El enemigo ganó la ronda!";
            }
        }
        else
        {
            textoResultado.text = "¡Defensa exitosa!";
        }

        turnoJugador = !turnoJugador;
        cartaAtacante = null;
        cartaDefensora = null;
        rondasCompletas++;
        ActualizarUI();
        VerificarFin();

        if (!turnoJugador)
        {
            Invoke("TurnoEnemigo", 2f);
        }
    }

    void TurnoEnemigo()
    {
        indexAtacante = BuscarSiguienteCartaEnemigo();
        if (indexAtacante == -1) return;

        cartaAtacante = cartasEnemigo[indexAtacante];
        textoEstado.text = "Te toca defenderte";
        textoResultado.text = "El enemigo ataca...";
    }

    void ActualizarUI()
    {
        vidaJugadorText.text = "Vida Jugador: " + vidaJugador;
        vidaEnemigoText.text = "Vida Enemigo: " + vidaEnemigo;

        if (vidaJugador > 0 && vidaEnemigo > 0)
        {
            textoEstado.text = turnoJugador ? "Tu turno: Ataca" : "Tu turno: Defiéndete";
        }
    }

    void VerificarFin()
    {
        if (vidaJugador <= 0)
        {
            SceneManager.LoadScene("derrota");
        }
        else if (vidaEnemigo <= 0)
        {
            SceneManager.LoadScene("Fin");
        }
        else if (rondasCompletas >= cantidadCartas)
        {
            if (vidaJugador > vidaEnemigo)
            {
                SceneManager.LoadScene("Fin");
            }
            else
            {
                SceneManager.LoadScene("derrota");
            }
        }
    }
}
