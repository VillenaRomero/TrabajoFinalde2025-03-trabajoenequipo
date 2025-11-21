using UnityEngine;
using DG.Tweening;

public class DotweenObject : MonoBehaviour
{
    [SerializeField]private RectTransform rtTexto;

    public Vector3 moved;
    public float duration = 1f; 

    private void Start()
    {
        rtTexto.DOLocalMove(moved, duration).SetEase(Ease.OutQuad);

    }
}
