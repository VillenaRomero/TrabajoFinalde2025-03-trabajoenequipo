using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIandObjectMoved : MonoBehaviour
{
    [SerializeField] private RectTransform Objecttransform;

    public Vector3 moved;
    public float duration = 1f; 

    private void Start()
    {
        Objecttransform.DOLocalMove(moved, duration).SetEase(Ease.OutQuad);
    }
}
