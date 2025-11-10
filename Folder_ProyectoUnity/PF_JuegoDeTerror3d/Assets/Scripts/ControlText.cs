using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    [SerializeField] private templatetext plantilla;
    [SerializeField] templatetext[] arrayplantilla;

    [SerializeField] TMP_Text Textnarar;
    [SerializeField] TMP_Text textanswerone;
    [SerializeField] TMP_Text textanswertwo;
    [SerializeField] TMP_Text textanswertree;

    [SerializeField] GameObject[] arraybutton;

    private void Start()
    {
        plantilla = arrayplantilla[0];
        mostrartexto();
    }
    void mostrartexto() {

        Textnarar.text = plantilla.textNarrar;
        textanswerone.text = plantilla.answerone;
        textanswertwo.text = plantilla.ansewtwo;
        textanswertree.text = plantilla.ansewtree;
    }
    public void controlButton(int indice)
    {
        plantilla = arrayplantilla[plantilla.arrayreference[indice]];
        if(plantilla.quitbutton == true)
        {
            desactivarbutton();
        }
        mostrartexto();
    }

    void desactivarbutton()
    {
        foreach (var boton in arraybutton) {
            boton.SetActive(false);
        }
    }
}
