using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [SerializeField] private GameObject reference;
    [SerializeField] private float Timewait;
    private float contador = 0f;
    [SerializeField] private float Timewait1;
    private float contador1 = 0f;
    private bool objectbool;

    void Update()
    {
        temp();
    }
    public void temp() { 
    if (objectbool == true)
        {
            contador += Time.deltaTime;

            if (contador >= Timewait)
            {
                objectbool = false;
            }
        }
        else if (objectbool == false) {
            contador += Time.deltaTime;
            if (contador >= Timewait) {

                objectbool = true;
                if (objectbool == true) {
                    contador1 += Time.deltaTime;

                    if (contador1 >= Timewait1)
                    {
                        objectbool = false;
                    }
                }
            }
        }
    }
}
