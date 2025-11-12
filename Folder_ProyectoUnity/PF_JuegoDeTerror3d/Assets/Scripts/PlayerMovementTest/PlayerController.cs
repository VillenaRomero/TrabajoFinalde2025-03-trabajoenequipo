using System;
using UnityEngine;

public enum MovementState
{
    Idle,
    Walking,
    Running,
    Crouching
}

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("~ Movement Settings")]
    [SerializeField] private bool enableMovement = true;
    [SerializeField] private float normalSpeed = 5f;

    [Header("~ Run Settings")]
    [SerializeField] private bool enableRun = true;
    [SerializeField] private float runSpeed = 10f;

    [Header("~ Crouch Settings")]
    [SerializeField] private bool enableCrouch = true;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("~ References")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody _playerRB;
    private float _actualSpeed;
    private Vector2 _movementInput;
    private MovementState _currentMovementState = MovementState.Idle;
    #endregion

    #region Properties
    public bool EnableMovement => enableMovement;
    public bool EnableRun => enableRun;
    public bool EnableCrouch => enableCrouch;
    public float NormalSpeed => normalSpeed;
    public float RunSpeed => runSpeed;
    public float CrouchSpeed => crouchSpeed;
    public Transform CameraTransform => cameraTransform;

    public Rigidbody PlayerRB => _playerRB;
    public float ActualSpeed => _actualSpeed;
    public Vector2 MovementInput => _movementInput;

    public void SetMoveSpeed(float newSpeed) => _actualSpeed = newSpeed;
    public void SetMovementInput(Vector2 input) => _movementInput = input;
    #endregion

    #region Events
    public static event Action<MovementState> OnMovementStateChange;
    public static event Action OnCrouchZoneExit;
    #endregion

    [SerializeField] private CapsuleCollider TopCollider;
    private bool _forceCrouchZone = false;
    public bool IsInCrouchZone => _forceCrouchZone;

    private void OnEnable()
    {
        OnMovementStateChange += UpdateSpeedByState;
    }
    private void OnDisable()
    {
        OnMovementStateChange -= UpdateSpeedByState;
    }

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _actualSpeed = normalSpeed;
    }

    private void Update()
    {
        UpdateTopColliderTrigger();
    }

    private void UpdateTopColliderTrigger()
    {
        TopCollider.isTrigger = _currentMovementState == MovementState.Crouching || _forceCrouchZone;
    }

    private void UpdateSpeedByState(MovementState state)
    {
        if (state == MovementState.Running && EnableRun)
        {
            SetMoveSpeed(RunSpeed);
        }
        else if (state == MovementState.Crouching && EnableCrouch)
        {
            SetMoveSpeed(CrouchSpeed);
        }
        else
        {
            SetMoveSpeed(NormalSpeed);
        }
    }

    public void UpdateMovementState(MovementState newState)
    {
        if (_currentMovementState != newState)
        {
            _currentMovementState = newState;
            OnMovementStateChange?.Invoke(_currentMovementState);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CrouchZone"))
        {
            _forceCrouchZone = true;
            UpdateMovementState(MovementState.Crouching);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrouchZone"))
        {
            _forceCrouchZone = false;
            OnCrouchZoneExit?.Invoke();
        }
    }
}
