using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

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

    private EventInstance AstHitAsteroid; //ganz viele Sounds kommen jetzt hier her
    private EventInstance AstWin;
    private EventInstance AstGameOver;

    private AudioManager script_AudioManager; //Referenz zu "Audiomanager", um Musik anzuhalten

    private GameObject spawner;                    // NEW Reference the spawner so that we can turn it off when win/loos
 
    private void Start()
    {
        UpdateScoreText();
        UpdatePlayerLives();
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false); 
        scoreCanvas.SetActive(true);

        spawner = GameObject.Find("Spawner");

        AstHitAsteroid = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstHitAsteroid); //Sound
        AstWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstWin);
        AstGameOver = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.AstGameOver);

        script_AudioManager = GameObject.Find("AudioManagerMusic").GetComponent<AudioManager>(); //Referenz zu AusiomanagerMusik Component mit "AudioManager" Skript
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        AstHitAsteroid.start(); //Sound

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
        playerLives.text = "Leben: " + this.lives;
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        if (this.lives <= 0)
        {
            Invoke(nameof(GameOver), 2f);
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
        spawner.SetActive(false);
        loseCanvas.SetActive(true);
        scoreCanvas.SetActive(false);
        player.winLooseOn = true;
        AstGameOver.start(); //Sound

        script_AudioManager.StopCurrentTheme(); //Musik stoppen
    }

    // New method to check if the player has won
    private void CheckForWin()
    {
        if (score >= winScoreThreshold)
        {
            FindObjectOfType<Asteroid_Player>().gameObject.SetActive(false);
            WinGame();
        }
    }

    // Method to trigger the win state
    private void WinGame()
    {
        //--------------------------------------------------------------------------------------------------------------------------------
        //---------Adjust Triggers and Goals----------------------------------------------------------------------------------------------------

        foreach (DataManager.TriggerableObj Trigger in DataManager.Triggerable_List)            //Activate New Eliza after winning Frogger
        {
            if (Trigger.Stored_ID == 43)
            {
                Trigger.Stored_Lock_State = false;
                break;
            }
        }


        foreach (DataManager.ActiveGoal Goal in DataManager.ActiveGoal_List)
        {
            if (Goal.Stored_ID == 5)
            {
                Goal.Stored_Completed = true;
            }
        }

        DataManager.ActiveGoal_List.Add(new DataManager.ActiveGoal { Stored_ID = 7, Stored_Completed = false });      //add Goal

        GloveScript.CallGlove = true;
        GloveScript.GloveProgress = 1;

        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------

        spawner.SetActive(false);
        player.winLooseOn = true;
        winCanvas.SetActive(true);  // Show the win screen
        scoreCanvas.SetActive(false); // Hide the score screen
        // You can add more functionality here, such as stopping asteroids, animations, etc.
        AstWin.start(); //Sound
        script_AudioManager.StopCurrentTheme(); //Musik stoppen
    }
}
