using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAlMenu : MonoBehaviour//->remover creo (villena)
{
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";

    public void IrAlMenu()
    {
       /* if (GameManagerPersonaje.instance != null)
        {
            GameManagerPersonaje.instance.ResetSeleccion();
        }*/

        SceneManager.LoadScene(nombreEscenaMenu);
    }
}
