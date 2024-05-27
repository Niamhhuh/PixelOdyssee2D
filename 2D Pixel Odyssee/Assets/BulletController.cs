using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // Geschwindigkeit des Geschosses
    private Vector3 direction; // Richtung des Geschosses

    // Methode zum Setzen der Bewegungsrichtung
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; // Normalisiere die Richtung, um die Geschwindigkeit konstant zu halten
    }

    void Update()
    {
        // Bewege das Geschoss in die festgelegte Richtung mit der festgelegten Geschwindigkeit
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
