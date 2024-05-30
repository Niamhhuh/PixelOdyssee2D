using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    public GameObject explosionEffect; // Prefab für den Explosionseffekt
    public GameObject rocketPrefab; // Prefab für die Rakete

    public float respawnTime = 2f; // Zeit bis zur Wiederbelebung der Rakete
    public Transform[] spawnPoints; // Array von Spawn-Punkten für die Raketen

    private Rigidbody2D rb;
    private bool isDestroyed = false; // Gibt an, ob die aktuelle Rakete zerstört wurde

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Starte den Respawn-Prozess
        StartCoroutine(RespawnRocket());
    }

    IEnumerator RespawnRocket()
    {
        Debug.Log("RespawnRocket Coroutine started.");

        while (true)
        {
            // Warte bis die aktuelle Rakete zerstört wurde
            yield return new WaitUntil(() => isDestroyed);

            // Wähle einen zufälligen Spawn-Punkt aus der Liste aus
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instanziiere eine Rakete am zufälligen Spawn-Punkt
            GameObject rocket = Instantiate(rocketPrefab, spawnPoint.position, spawnPoint.rotation);
            HomingMissile homingMissile = rocket.GetComponent<HomingMissile>();
            if (homingMissile != null)
            {
                // Setze das Ziel der neuen Rakete
                homingMissile.target = target;
            }

            // Setze die Initialgeschwindigkeit der Rakete
            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = rocket.transform.up * speed;
            }

            // Setze den Status der aktuellen Rakete zurück
            isDestroyed = false;
        }

        Debug.Log("RespawnRocket Coroutine finished.");
    }

    void FixedUpdate()
    {
        if (target != null && rb != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Debugging: Log the position where the explosion should occur
            Debug.Log("Missile hit at position: " + transform.position);

            // Instanziiere den Explosionseffekt an der aktuellen Position und Rotation der Rakete
            if (explosionEffect != null)
            {
                // Instanziiere den Effekt
                GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

                // Setze die lokale Position und Rotation zurück
                explosion.transform.localPosition = Vector3.zero;
                explosion.transform.localRotation = Quaternion.identity;

                // Manuelles Starten des Video Players, falls vorhanden
                VideoPlayer videoPlayer = explosion.GetComponent<VideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.Play();
                    Destroy(explosion, (float)videoPlayer.clip.length); // Zerstöre den Effekt nach der Länge des Videos
                }

                // Manuelles Starten des Partikelsystems, falls vorhanden
                ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                    Destroy(explosion, particleSystem.main.duration); // Zerstöre den Effekt nach der Dauer des Partikelsystems
                }
            }
            else
            {
                Debug.LogWarning("Explosion effect prefab is not assigned!");
            }

            // Setze den Status der aktuellen Rakete auf zerstört
            isDestroyed = true;

            // Coroutine neu starten
            StartCoroutine(RespawnRocket());

            // Zerstöre die Rakete
            Destroy(gameObject);
        }
    }
}
