using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class playermoved : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed;
    public float rotationSpeed = 10f;

    [Header("Vida")]
    [SerializeField] private int currentLife = 100;
    public int maxLife = 10;
    public Slider lifeSlider;

    [Header("Dash")]
    public float dashMultiplier = 2f;
    public float dashStaminaCost = 25f;

    [Header("Estamina")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaRegenRate = 15f;
    public Slider staminaSlider;

    [Header("Inventario")]
    public GameObject[] inventario = new GameObject[5];
    private int indiceObjeto = 0;

    private Vector2 moveInput;
    private Camera mainCam;
    private bool isDashing = false;
    private bool isHiding = false;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
        if (lifeSlider != null)
        {
            lifeSlider.maxValue = maxLife;
            lifeSlider.value = currentLife;
        }
    }

    void Update()
    {
        if (!isHiding)
        {
            MoveWithCameraDirection(); // jugador se mueva en la dirección en que apunta la cámara
            PlayerLookAtMouse(); // jugador mire hacia el puntero del mouse
        }

        if (Input.GetKeyDown(KeyCode.Q))
            UsarObjetoActual();

        if (Input.GetKeyDown(KeyCode.E))
            CambiarObjetoDerecha();

        if (Input.GetKeyDown(KeyCode.F))
            CambiarObjetoIzquierda();

        RegenerarStamina();
        UpdateStaminaUI();
        UpdateLifeUI();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && stamina >= dashStaminaCost)
        {
            isDashing = true;
            stamina -= dashStaminaCost * Time.deltaTime;
        }
        else if (ctx.canceled)
        {
            isDashing = false;
        }
    }

    void MoveWithCameraDirection()  // jugador se mueva en la dirección en que apunta la cámara
    {
        if (mainCam == null) return;

        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        float lifeRatio = Mathf.Clamp01((float)currentLife / maxLife);
        float currentSpeed = speed * lifeRatio; // velocidad depende de la vida
        if (isDashing) currentSpeed *= dashMultiplier;

        transform.position += moveDir * currentSpeed * Time.deltaTime;
    }

    void PlayerLookAtMouse()//jugador mire hacia el puntero del mouse
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = (hitPoint - transform.position);
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void RegenerarStamina()
    {
        if (!isDashing && stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null) staminaSlider.value = stamina;
    }

    void UpdateLifeUI()
    {
        if (lifeSlider != null) lifeSlider.value = currentLife;
    }

    void UsarObjetoActual()
    {
        if (inventario[indiceObjeto] != null)
        {
            inventario[indiceObjeto].SetActive(true);
            Debug.Log("Usando: " + inventario[indiceObjeto].name);
        }
        else
        {
            Debug.Log("No hay objeto");
        }
    }

    void CambiarObjetoDerecha()
    {
        indiceObjeto++;
        if (indiceObjeto >= inventario.Length) indiceObjeto = 0;

        if (inventario[indiceObjeto] != null)
            Debug.Log("Objeto: " + inventario[indiceObjeto].name);
        else
            Debug.Log("Objeto: vacío");
    }

    void CambiarObjetoIzquierda()
    {
        indiceObjeto--;
        if (indiceObjeto < 0) indiceObjeto = inventario.Length - 1;

        if (inventario[indiceObjeto] != null)
            Debug.Log("Objeto: " + inventario[indiceObjeto].name);
        else
            Debug.Log("Objeto: vacío");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            for (int i = 0; i < inventario.Length; i++)
            {
                if (inventario[i] == null)
                {
                    inventario[i] = other.gameObject;
                    inventario[i].SetActive(false);
                    Debug.Log("Objeto recogido: " + other.name);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        if (currentLife < 0) currentLife = 0;

        speed = Mathf.Clamp((5f * currentLife) / maxLife, 1f, 5f);

        Debug.Log("Vida: " + currentLife + " | Velocidad: " + speed);
    }
}
