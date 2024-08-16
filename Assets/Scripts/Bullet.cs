using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsServer)
        {
            rb.velocity = direction * speed;
            Destroy(gameObject, lifetime);
        }
    }

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;

        if (IsServer)
        {
            rb.velocity = direction * speed;
            Destroy(gameObject, lifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer && other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
