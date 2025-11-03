using UnityEngine;
using UnityEngine.SceneManagement;

public class escenaMenu : MonoBehaviour//->Mochalo
{
    public string nombreEscena ;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
