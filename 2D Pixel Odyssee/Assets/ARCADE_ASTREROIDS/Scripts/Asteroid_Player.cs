using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Player : MonoBehaviour
{
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
        
}
