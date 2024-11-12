using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Asteroid_GameManager : MonoBehaviour
{
    public Asteroid_Player player;
    public ParticleSystem explosion;
    public TextMeshProUGUI scoreText; // Use TextMeshProUGUI instead of Text

    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;

    public int lives = 3;
    public int score = 0;

    private void Start()
    {
        // Initialize the score display at the start of the game
        UpdateScoreText();
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 1.75f)
        {
            this.score += 100;
        }
        else if (asteroid.size < 2.0f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }

        // Update the score display
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + this.score;
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    public void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("AsteroidPlayer");
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}
