using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform center; // Zentrum des Kreises
    public float radius = 5f; // Radius des Kreises
    public float speed = 2f; // Geschwindigkeit des Spielers
    public float rotationSpeed = 50f; // Rotationsgeschwindigkeit
    public float maxAttractionForce = 2f; // Maximale Stärke der Anziehungskraft zum Zentrum
    public float attractionIncrement = 0.8f; // Inkrement für die Erhöhung der Anziehungskraft

    private float angle = 0f;
    private float currentAttractionForce = 0f;
    SWSoundManager SWSoundManager;

    private void Start()
    {
        SWSoundManager = GameObject.FindGameObjectWithTag("SoundSpaceWar").GetComponent<SWSoundManager>();
    }
    void Update()
    {
        // Rotation mit den Pfeiltasten
        float rotationInput = -Input.GetAxis("Horizontal"); // Negativer Wert für umgekehrte Richtung
        transform.Rotate(Vector3.forward, rotationInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D))
        {
            SWSoundManager.PlaySfxSW(SWSoundManager.BoostSW);
        }


        // Schießen mit der Leertaste
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Hier nichts tun oder andere Logik implementieren, falls erforderlich
        }

        // Berechne den neuen Winkel basierend auf der Geschwindigkeit
        angle += speed * Time.deltaTime;

        // Berechne die neue Position des Spielers auf dem Kreis
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float y = center.position.y + Mathf.Sin(angle) * radius;

        // Berechne die Richtung zum Zentrum
        Vector2 directionToCenter = ((Vector2)center.position - new Vector2(x, y)).normalized;

        // Erhöhe die Anziehungskraft schrittweise
        currentAttractionForce = Mathf.Min(currentAttractionForce + attractionIncrement, maxAttractionForce);

        // Berechne die Anziehungskraft zum Zentrum
        Vector2 attraction = directionToCenter * currentAttractionForce;

        // Berechne die endgültige Position des Spielers mit Anziehungskraft
        x += attraction.x;
        y += attraction.y;

        // Setze die Position des Spielers
        transform.position = new Vector2(x, y);
    }
}
