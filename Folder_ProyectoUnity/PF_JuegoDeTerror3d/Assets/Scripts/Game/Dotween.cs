using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Dotween : MonoBehaviour
{
    [SerializeField]private TMP_Text texto;
    [SerializeField]private Image imagen;

    public Vector3 moved;
    public Vector3 moved1;
    public float duration = 1f; 

    public Vector3 rotacionZ;
    public float tiempoRotacion = 0.4f;

    private RectTransform rtTexto;
    private RectTransform rtImagen;

    private void Start()
    {
        if (texto != null) rtTexto = texto.rectTransform;
        if (imagen != null) rtImagen = imagen.rectTransform;

        rtTexto.DOLocalMove(moved, duration).SetEase(Ease.OutQuad);
        rtImagen.DOLocalMove(moved1, duration).SetEase(Ease.OutQuad);

        rtImagen.DOLocalRotate(rotacionZ, tiempoRotacion).SetEase(Ease.InOutSine);
    }
}
