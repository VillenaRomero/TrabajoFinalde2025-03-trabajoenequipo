using UnityEngine;
using DG.Tweening;

public class GridController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private AnimationCurve enterAnimationCurve;
    [SerializeField] private AnimationCurve exitAnimationCurve;

    [Header("Settings")]
    [SerializeField] private Vector3 enterRotation = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 exitRotation = new Vector3(0, 0, 0);
    [SerializeField] private float enterduration = 0.4f;
    [SerializeField] private float exitduration = 0.7f;

    private Tween _currentTween;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _currentTween?.Kill();
            _currentTween = pivotTransform.DORotate(enterRotation, enterduration).SetEase(enterAnimationCurve);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _currentTween?.Kill();
            _currentTween = pivotTransform.DORotate(exitRotation, exitduration).SetEase(exitAnimationCurve);
        }
    }
}
