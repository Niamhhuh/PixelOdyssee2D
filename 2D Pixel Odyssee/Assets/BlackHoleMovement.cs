using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public Transform center; // Zentrum des Kreises
    public float radius = 5f; // Radius des Kreises
    public float speed = 2f; // Geschwindigkeit des Spielers

    private float angle = 0f;

    void Update()
    {
        // Berechne den neuen Winkel basierend auf der Geschwindigkeit
        angle += speed * Time.deltaTime;

        // Berechne die neue Position des Spielers auf dem Kreis
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;

        // Setze die Position des Spielers
        transform.position = new Vector2(x, y);
    }
}







  

    