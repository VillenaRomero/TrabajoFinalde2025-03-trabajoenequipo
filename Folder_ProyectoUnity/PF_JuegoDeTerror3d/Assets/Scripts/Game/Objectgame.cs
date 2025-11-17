using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Objectgame : MonoBehaviour
{
    public TMP_Text textoDialogo;
    public string mensaje;

    private void Start()
    {
        textoDialogo.text = mensaje;
        textoDialogo.gameObject.SetActive(false);
    }


    public void MostrarDialogo()
    {
        if (textoDialogo != null)
        {
            textoDialogo.gameObject.SetActive(true);
        }
    }

    public void OcultarDialogo()
    {
        if (textoDialogo != null)
        {
            textoDialogo.gameObject.SetActive(false);
        }
    }
}
