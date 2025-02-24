using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;

    public float size = 4.0f; // Increased initial size
    public float minSize = 1.0f; // Reduced minimum size for more splits
    public float maxSize = 5.0f; // Allow larger initial asteroids

    public float speed = 50.0f;

    public float maxLifetime = 30.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Randomize the asteroid's appearance
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        // Randomize its rotation
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);

        // Set its size
        this.transform.localScale = Vector3.one * this.size;

        // Adjust its mass based on size
        _rigidbody.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with a bullet
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject); // Ensure the bullet is destroyed immediately

            if ((this.size * 0.5f) > this.minSize)
            {
                // Create two smaller asteroids
                CreateSplit();
                CreateSplit();
            }

            // Notify the game manager and destroy this asteroid
            FindObjectOfType<Asteroid_GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
        //-------------------------------------------------------------------------------------------------------------------
        // Check if collided with deathzone  ------>> added by Kimi as a replacement for the asteroids disappearing
        if (collision.gameObject.tag == "deathzone")
        {
            // Notify the game manager and destroy this asteroid
            Destroy(this.gameObject);
        }
    }


    private void CreateSplit()
    {
        // Slightly offset the position for the new asteroid
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Create a new asteroid with half the size
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f; // Halve the size
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
