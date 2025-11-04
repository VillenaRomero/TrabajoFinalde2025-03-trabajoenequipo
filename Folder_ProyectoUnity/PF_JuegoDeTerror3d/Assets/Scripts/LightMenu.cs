using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightMenu : MonoBehaviour
{

    public Light light3D;
    public float minIntensity;
    public float maxIntensity;
    public float flickerSpeed;
    public float offChance;

    private float timer;

    public TMP_Text[] textostmpo;
    public Color color;

    void Start()
    {
        if (light3D == null)
            light3D = GetComponent<Light>();
        color.a = 10f;
    }

    void Update()
    {
        Efectlight();
        
    }
    void Efectlight(){
        timer += Time.deltaTime;

        if (timer >= flickerSpeed)
        {
            if (Random.value < offChance)
            {
                light3D.intensity = 0f;
            }
            else
            {
                light3D.intensity = Random.Range(minIntensity, maxIntensity);
                color.a = Random.Range(minIntensity, maxIntensity);
            }

            timer = 0f;
        }
    }
}
