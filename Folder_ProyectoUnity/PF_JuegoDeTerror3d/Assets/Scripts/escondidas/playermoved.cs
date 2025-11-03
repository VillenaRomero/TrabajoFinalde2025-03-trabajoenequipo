using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class playermoved : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Vida")]
    [SerializeField] private int currentLife = 100;
    public int maxLife = 10;
    public Slider lifeSlider;

    [Header("Dash")]
    public float dashForce = 10f;
    public int dashStaminaCost = 25;
    private bool isDashing = false;
    private float dashCooldown = 0.5f;
    private float dashTimer;

    [Header("Estamina")]
    public int stamina = 100;
    public int maxStamina = 100;
    public float staminaRegenDelay = 2f;
    public float staminaRegenSpeed = 10f;
    public Slider staminaSlider;
    private float regenTimer;

    [Header("Inventario")]
    public GameObject[] inventario = new GameObject[5];
    private int indiceObjeto = 0;

    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        MoverJugador();

        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

        if (!isDashing)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= staminaRegenDelay && stamina < maxStamina)
            {
                stamina += (int)(staminaRegenSpeed * Time.deltaTime);
                if (stamina > maxStamina) stamina = maxStamina;
            }
        }
        else
        {
            regenTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            UsarObjetoActual();

        if (Input.GetKeyDown(KeyCode.E))
            CambiarObjetoDerecha();

        if (Input.GetKeyDown(KeyCode.F))
            CambiarObjetoIzquierda();

        UpdateStaminaUI();
        UpdateLifeUI();

        // Dash con tecla Shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
            HacerDash();
    }

    void MoverJugador()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Vector3 move = moveDir.normalized * speed;
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

            Quaternion rot = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Lerp(rb.rotation, rot, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void HacerDash()
    {
        if (stamina >= dashStaminaCost && dashTimer <= 0)
        {
            isDashing = true;
            stamina -= dashStaminaCost;
            dashTimer = dashCooldown;

            rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);

            isDashing = false;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void RegenerarStamina()
    {
        if (!isDashing && stamina < maxStamina)
        {
            stamina += (int)(staminaRegenSpeed * Time.deltaTime);
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
    }

    void CambiarObjetoDerecha()
    {
        indiceObjeto++;
        if (indiceObjeto >= inventario.Length) indiceObjeto = 0;

        if (inventario[indiceObjeto] != null)
            Debug.Log("Objeto: " + inventario[indiceObjeto].name);
      
    }

    void CambiarObjetoIzquierda()
    {
        indiceObjeto--;
        if (indiceObjeto < 0) indiceObjeto = inventario.Length - 1;

        if (inventario[indiceObjeto] != null)
            Debug.Log("Objeto: " + inventario[indiceObjeto].name);
        
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
