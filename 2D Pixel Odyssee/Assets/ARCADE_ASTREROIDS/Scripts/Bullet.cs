using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public float maxLifetime = 10.0f;
    public float speed = 500.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime); // Destroy after maxLifetime
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hits a boundary
        if (other.CompareTag("Boundary"))
        {
            Destroy(this.gameObject); // Destroy the bullet on collision with a boundary
        }
    }
}
