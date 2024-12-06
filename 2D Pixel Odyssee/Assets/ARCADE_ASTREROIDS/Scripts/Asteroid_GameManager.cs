using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Asteroid_GameManager : MonoBehaviour
{
    public Asteroid_Player player;
    public ParticleSystem explosion;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerLives;

    public GameObject loseCanvas;
    public GameObject winCanvas; // Add a UI canvas for the win state
    public GameObject scoreCanvas;

    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;

    public int lives = 3;
    public int score = 0;

    // Win state variables
    public int winScoreThreshold = 2000; // Define the score threshold to win

    private void Start()
    {
        UpdateScoreText();
        UpdatePlayerLives();
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false); 
        scoreCanvas.SetActive(true);
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

        // Check if the player has won
        CheckForWin();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + this.score;
    }

    private void UpdatePlayerLives()
    {
        playerLives.text = "Lives: " + this.lives;
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

        UpdatePlayerLives();
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
        loseCanvas.SetActive(true);
        scoreCanvas.SetActive(false);
    }

    // New method to check if the player has won
    private void CheckForWin()
    {
        if (score >= winScoreThreshold)
        {
            WinGame();
        }
    }

    // Method to trigger the win state
    private void WinGame()
    {
        winCanvas.SetActive(true);  // Show the win screen
        scoreCanvas.SetActive(false); // Hide the score screen
        Time.timeScale = 0; // Optionally stop the game or pause
        // You can add more functionality here, such as stopping asteroids, animations, etc.
    }

    // Optionally, create methods to restart the game or quit after winning
    public void RestartGame()
    {
        Time.timeScale = 1; // Unpause the game
        // Reset the game state, scores, lives, etc.
        lives = 3;
        score = 0;
        UpdatePlayerLives();
        UpdateScoreText();
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application (works in build)
    }
}
