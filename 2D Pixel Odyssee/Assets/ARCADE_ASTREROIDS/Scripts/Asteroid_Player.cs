using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class Asteroid_Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    public TextMeshProUGUI panicTeleportText;

    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private float _turnDirection;

    private Transform boundaryTop, boundaryBottom, boundaryLeft, boundaryRight;

    private float panicTeleportCooldown = 20.0f; // Cooldown in seconds
    private float currentCooldownTime = 0.0f; // Time remaining for cooldown

    private EventInstance AstShot; //ganz viele Sounds kommen jetzt hier her
    private EventInstance AstTeleport;
    private EventInstance AstBoost;
    private EventInstance AstHitPlayer;

    public bool winLooseOn;

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

        AstShot = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstShot); //Sound
        AstTeleport = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstTeleport);
        AstBoost = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstBoost);
        AstHitPlayer = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstHitPlayer);

        winLooseOn = false;

        UpdatePanicTeleportText();
    }

    private void Update() {
        if (winLooseOn == false) {
            Movement();
        }
    }

    public void Movement() {                // Moved from Update() to over here so that we can disable it --> scuffed fix but fast
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
            AstBoost.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //SoundStop
        }

        // Handle shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Handle Panic Teleport with cooldown check
        if (Input.GetKeyDown(KeyCode.Q) && currentCooldownTime <= 0.0f)
        {
            PanicTeleport();
            AstTeleport.start(); //Sound
        }

        // Update cooldown timer
        if (currentCooldownTime > 0.0f)
        {
            currentCooldownTime -= Time.deltaTime;
            UpdatePanicTeleportText(); // Update the UI with remaining cooldown time
        }
    }

    private void FixedUpdate()
    {
        // Apply thrust
        if (_thrusting)
        {
            _rigidbody.AddForce(transform.up * thrustSpeed);
            AstBoost.start();
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
        AstShot.start(); //Sound
    }

    private void PanicTeleport()
    {
        // Generate a random position within the screen boundaries
        float randomX = Random.Range(boundaryLeft.position.x + 1.5f, boundaryRight.position.x - 1.5f);
        float randomY = Random.Range(boundaryBottom.position.y + 1.5f, boundaryTop.position.y - 1.5f);

        // Set the player's position to the new random location
        transform.position = new Vector3(randomX, randomY, transform.position.z);

        // Reset velocity to avoid drifting after teleport
        //_rigidbody.velocity = Vector2.zero;
        //_rigidbody.angularVelocity = 0.0f;

        // Start the cooldown timer
        currentCooldownTime = panicTeleportCooldown;

        // Update the UI
        UpdatePanicTeleportText();

        Debug.Log("Teleported to: " + transform.position);
    }

    private void UpdatePanicTeleportText()
    {
        if (currentCooldownTime > 0.0f)
        {
            panicTeleportText.text = "Teleport: USED";
            panicTeleportText.color = Color.red;
        }
        else
        {
            panicTeleportText.text = "Teleport: READY";
            panicTeleportText.color = Color.green;
        }
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
            AstHitPlayer.start(); //Sound
            AstBoost.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //SoundStop
        }
    }
}
