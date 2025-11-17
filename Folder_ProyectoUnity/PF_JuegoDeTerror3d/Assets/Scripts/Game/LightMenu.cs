using TMPro;
using UnityEngine;
using DG.Tweening;

public class LightMenu : MonoBehaviour
{
    [Header("Light Flicker Settings")]
    public Light light3D;
    public float minIntensity;
    public float maxIntensity;
    public float flickerSpeed;
    public float offChance;

    [Header("Pendulum Settings")]
    [SerializeField] private float pendulumAngle = 30f;
    [SerializeField] private float pendulumDuration = 2f;
    [SerializeField] private AnimationCurve pendulumCurve;
    [SerializeField] private PendulumAxis rotationAxis = PendulumAxis.Z;

    private float timer;
    private Tween _pendulumTween;
    private Vector3 _initialRotation;

    public TMP_Text[] textostmpo;
    public Color color;

    private enum PendulumAxis { X, Y, Z }

    void Start()
    {
        if (light3D == null)
            light3D = GetComponent<Light>();
        color.a = 10f;

        _initialRotation = transform.localEulerAngles;
        StartPendulumMotion();
    }

    void Update()
    {
        Efectlight();
    }

    void Efectlight()
    {
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

    private void StartPendulumMotion()
    {
        _pendulumTween?.Kill();

        Vector3 targetRotation = _initialRotation;

        switch (rotationAxis)
        {
            case PendulumAxis.X:
                targetRotation = new Vector3(_initialRotation.x + pendulumAngle, _initialRotation.y, _initialRotation.z);
                break;
            case PendulumAxis.Y:
                targetRotation = new Vector3(_initialRotation.x, _initialRotation.y + pendulumAngle, _initialRotation.z);
                break;
            case PendulumAxis.Z:
                targetRotation = new Vector3(_initialRotation.x, _initialRotation.y, _initialRotation.z + pendulumAngle);
                break;
        }

        _pendulumTween = transform.DOLocalRotate(targetRotation, pendulumDuration).SetEase(pendulumCurve).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _pendulumTween?.Kill();
    }
}
