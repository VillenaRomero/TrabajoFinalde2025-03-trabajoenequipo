using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPaused : MonoBehaviour
{
    [SerializeField] private GameObject botonpausa;
    [SerializeField] private GameObject menuPausa;

    public void Pausa()
    {
        Time.timeScale = 0f;
        botonpausa.SetActive(false);
        menuPausa.SetActive(true);
    }
    public void Reanudar()
    {
        Time.timeScale = 1f;
        botonpausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void Cerrar()
    {
        Application.Quit();
    }

    public void Cambiarescenio(string nombreescena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreescena);
    }
}
