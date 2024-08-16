using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public TMP_Text pickupCounterTextPrefab;
    public TMP_Text timerTextPrefab;
    public Transform namePosition;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private TMP_Text pickupCounterTextInstance;
    private TMP_Text timerTextInstance;
    private MyControls controls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping;
    private bool isGrounded;

    private NetworkVariable<int> pickupCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private float timer;
    private bool isTimerRunning = false;

    private void Awake()
    {
        controls = new MyControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Attack.performed += ctx => ShootBullet();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsLocalPlayer)
        {
            Canvas uiCanvas = GameObject.FindObjectOfType<Canvas>();
            if (uiCanvas == null)
            {
                return;
            }

            pickupCounterTextInstance = Instantiate(pickupCounterTextPrefab, uiCanvas.transform);
            pickupCounterTextInstance.gameObject.SetActive(true);

            timerTextInstance = Instantiate(timerTextPrefab, uiCanvas.transform);
            timerTextInstance.gameObject.SetActive(true);

            StartTimer();
        }
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            UpdatePickupCounterUI();
            UpdateTimerUI();
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector2 velocity = rb.velocity;
        velocity.x = moveInput.x * speed;
        rb.velocity = velocity;

        if (isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            isJumping = true;
        }
    }
    public void CollectPickup()
    {
        if (IsOwner)
        {
            CollectPickupServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void CollectPickupServerRpc()
    {
        pickupCount.Value++;
    }

    private void UpdatePickupCounterUI()
    {
        if (pickupCounterTextInstance != null)
        {
            pickupCounterTextInstance.text = "Pickups: " + pickupCount.Value;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerTextInstance != null)
        {
            if (isTimerRunning)
            {
                timer += Time.deltaTime;
                timerTextInstance.text = "Time: " + Mathf.FloorToInt(timer).ToString();
            }
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void ShootBullet()
    {
        if (IsLocalPlayer)
        {
            ShootBulletServerRpc();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 shootDirection = (mousePosition - (Vector2)bulletSpawn.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(shootDirection, 15f);
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5f);
    }
}
