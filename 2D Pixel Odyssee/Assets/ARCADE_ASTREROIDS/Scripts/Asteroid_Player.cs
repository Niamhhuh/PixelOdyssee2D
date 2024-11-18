using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Player : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private float _turnDirection;

    private Transform boundaryTop, boundaryBottom, boundaryLeft, boundaryRight;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Cache boundary references
        boundaryTop = GameObject.Find("BoundaryTop").transform;
        boundaryBottom = GameObject.Find("BoundaryBottom").transform;
        boundaryLeft = GameObject.Find("BoundaryLeft").transform;
        boundaryRight = GameObject.Find("BoundaryRight").transform;
    }

    private void Update()
    {
        // Handle thrust and rotation input
        _thrusting = Input.GetKey(KeyCode.W);

        if (Input.GetKey(KeyCode.A))
        {
            _turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _turnDirection = -1.0f;
        }
        else
        {
            _turnDirection = 0.0f;
        }

        // Handle shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Handle panic teleport
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PanicTeleport();
        }
    }

    private void FixedUpdate()
    {
        // Apply thrust
        if (_thrusting)
        {
            _rigidbody.AddForce(transform.up * thrustSpeed);
        }

        // Apply rotation
        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.up);
    }

    private void PanicTeleport()
    {
        // Generate a random position within the screen boundaries
        float randomX = Random.Range(boundaryLeft.position.x + 1.5f, boundaryRight.position.x - 1.5f);
        float randomY = Random.Range(boundaryBottom.position.y + 1.5f, boundaryTop.position.y - 1.5f);

        // Set the player's position to the new random location
        transform.position = new Vector3(randomX, randomY, transform.position.z);

        // Optionally reset velocity
        // _rigidbody.velocity = Vector2.zero;
        //_rigidbody.angularVelocity = 0.0f;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle screen wrapping
        if (other.gameObject.CompareTag("Boundary"))
        {
            Vector3 newPosition = transform.position;

            if (other.gameObject.name == "BoundaryLeft")
            {
                newPosition.x = boundaryRight.position.x - 1.5f; // Move to the right boundary
            }
            else if (other.gameObject.name == "BoundaryRight")
            {
                newPosition.x = boundaryLeft.position.x + 1.5f; // Move to the left boundary
            }
            else if (other.gameObject.name == "BoundaryTop")
            {
                newPosition.y = boundaryBottom.position.y + 1.5f; // Move to the bottom boundary
            }
            else if (other.gameObject.name == "BoundaryBottom")
            {
                newPosition.y = boundaryTop.position.y - 1.5f; // Move to the top boundary
            }

            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with asteroids
        if (collision.gameObject.tag == "Asteroid")
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            gameObject.SetActive(false);

            FindObjectOfType<Asteroid_GameManager>().PlayerDied();
        }
    }
}
