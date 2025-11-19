using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;
    [SerializeField] private float staminaRegenRate = 15f;
    [SerializeField] private float minStaminaToRun = 100f;
    [SerializeField] private Color fullStaminaColor;
    [SerializeField] private Color staminaColor;

    [Header("UI References")]
    [SerializeField] private Image runIcon;
    [SerializeField] private Image staminaCircle;

    private PlayerController _pc;
    private PlayerInputs _playerInputs;
    private float _currentStamina;
    private bool _canRun = true;

    public bool CanRun => _canRun;
    public float CurrentStamina => _currentStamina;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
        _playerInputs = GetComponent<PlayerInputs>();

        _currentStamina = maxStamina;
        staminaCircle.fillAmount = _currentStamina / 100;
    }

    private void Update()
    {
        UpdateStamina();
        UpdateUI();
    }

    private void UpdateStamina()
    {
        bool previousCanRun = _canRun;

        if (_pc.CurrentMovementState == MovementState.Running && _pc.IsMoving)
        {
            _currentStamina -= staminaDrainRate * Time.deltaTime;
            _currentStamina = Mathf.Max(_currentStamina, 0f);

            if (_currentStamina <= 0f)
            {
                _canRun = false;
                runIcon.color = staminaColor;
            }
        }
        else
        {
            _currentStamina += staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, maxStamina);

            if (_currentStamina >= minStaminaToRun)
            {
                _canRun = true;
                runIcon.color = fullStaminaColor;
            }
        }

        if ((_playerInputs.InputActions.Player.Run.IsPressed() == false && _currentStamina <= 100f))
        {
            if (_currentStamina < minStaminaToRun)
            {
                _canRun = false;
                runIcon.color = staminaColor;
            }
        }

        if (previousCanRun != _canRun && _playerInputs != null)
        {
            _playerInputs.ForceUpdateMovementState();
        }
    }

    private void UpdateUI()
    {
            staminaCircle.fillAmount = _currentStamina / 100;
    }
}