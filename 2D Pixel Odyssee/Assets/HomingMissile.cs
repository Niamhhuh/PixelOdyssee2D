using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    public GameObject explosionEffect; // Prefab für den Explosionseffekt

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Find target
        //target = GameObject.FindGameObjectWithTag("BlackHole").transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Instanziiere den Explosionseffekt
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Zerstoere die Rakete
        Destroy(gameObject);
    }
}
