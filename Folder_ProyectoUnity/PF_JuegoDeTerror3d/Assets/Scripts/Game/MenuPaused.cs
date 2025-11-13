using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPaused : MonoBehaviour //Lo puedes hacer en un menu manager general
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;

    public void Pausa()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void Reanudar()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
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
