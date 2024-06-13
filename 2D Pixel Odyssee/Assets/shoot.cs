using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab des Geschosses
    public Transform firePoint; // Ausgangspunkt des Geschosses
    SWSoundManager SWSoundManager;

    private void Awake()
    {
        SWSoundManager = GameObject.FindGameObjectWithTag("SoundSpaceWar").GetComponent<SWSoundManager>();
    }
    void Update()
    {
        // Schießen mit der Leertaste
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SWSoundManager.PlaySfxSW(SWSoundManager.ShootSW);
            FireBullet();
        }
    }

    // Methode zum Feuern eines Geschosses
    void FireBullet()
    {
        // Erstelle ein Geschoss aus dem Prefab am Feuerpunkt
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Verwende die Rotation des Feuerpunkts

        // Berechne die Bewegungsrichtung des Geschosses basierend auf der Rotation des Feuerpunkts
        Vector3 bulletDirection = firePoint.up; // Verwende die Up-Richtung des Feuerpunkts als Bewegungsrichtung

        // Hole das BulletController-Skript vom Geschoss und setze die Richtung
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.SetDirection(bulletDirection);
        }
        else
        {
            Debug.LogWarning("BulletController nicht gefunden");
        }
    }
}
