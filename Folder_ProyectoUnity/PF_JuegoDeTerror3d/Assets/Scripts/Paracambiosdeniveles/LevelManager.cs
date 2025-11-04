using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour//->SceneController
{
    public static SceneController Instance;

    [Header("Configuración de Niveles")]
    [SerializeField] private int totalLevels = 5; 
    private int nextLevelIndex = 2; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToNextLevel()
    {
        if (nextLevelIndex <= totalLevels)
        {
            SceneManager.LoadScene("Nivel" + nextLevelIndex);
        }
        else
        {
            SceneManager.LoadScene("EscenaFinal");
        }
    }

    public void WinLevel()
    {
        if (nextLevelIndex <= totalLevels)
        {
            nextLevelIndex++;
        }

        ReturnToLevel1();
    }

    public void ReturnToLevel1()
    {
        SceneManager.LoadScene("Nivel1");
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ResetProgress()
    {
        nextLevelIndex = 2;
    }
}
