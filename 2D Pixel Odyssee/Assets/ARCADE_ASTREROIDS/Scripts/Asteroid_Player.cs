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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _thrusting = (Input.GetKey(KeyCode.W));
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * thrustSpeed);
        }
        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
        }

    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Asteroid")
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            FindObjectOfType<Asteroid_GameManager>().PlayerDied();
        }
    }

}
