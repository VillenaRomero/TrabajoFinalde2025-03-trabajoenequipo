using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambiodeescena : MonoBehaviour//->Mochalo
{
    public void Cambiodeescenario(string name) {
        SceneManager.LoadScene(name);
    }
}
