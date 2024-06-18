using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    private Frogger frogger;
    
    private Home[] homes;
    
    private int score;

    private int lives;

    private int time;

    public GameObject gameOverMenu;

    public GameObject gameWonMenu;

    public Text livesText;

    SoundManager soundManager;

    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void Start()
    {
        soundManager.PlayMusic(soundManager.background);
        NewGame();  
    }

    private void NewGame()
    {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        NewRound();
        
    }

    private void NewLevel()
    {
        /*for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        NewRound();*/
        frogger.gameObject.SetActive(false);
        gameWonMenu.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(BackToHub());
    }

    private void NewRound() 
    {
        Respawn();
    }

    private void Respawn()
    {
        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        time = duration;

        while (time > 0) 
        { 
            yield return new WaitForSeconds(1);

            time --;
        }

        frogger.Death();
    }

    public void Died()
    {
        SetLives(lives - 1);    

        if (lives > 0)
        {
            soundManager.PlaySfx(soundManager.death);
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);   
        }
    }

    private void GameOver()
    {
        frogger.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);
        soundManager.StopMusic(soundManager.background);
        soundManager.PlaySfx(soundManager.gameOver);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());

    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;
        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playAgain = true;
            }

            yield return null;
        }
        NewGame();
    }

    private IEnumerator BackToHub(){

        bool BackToHub = false;
        while (!BackToHub)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                BackToHub = true;
            }

            yield return null;
        }
        SceneManager.LoadScene("TutorialRoom");
    }

    public void AdvanceRow()
    {
        SetScore(score + 10);
    }

    public void HomeOccupied()
    {
        frogger.gameObject.SetActive(false);

        int bonusPoints = time * 20;
        SetScore(score + bonusPoints + 50);
        soundManager.PlaySfx(soundManager.score);

        if (Cleared())
        {
            SetScore(score + 1000);
            SetLives(lives + 1);
            Invoke (nameof(NewLevel), 1f);
            soundManager.StopMusic(soundManager.background);
            soundManager.PlaySfx(soundManager.win);
        }
        else
        {
            Invoke (nameof(NewRound), 1f);
        }
    }

    private bool Cleared()
    {

        for (int i = 0; i<homes.Length; i++)
        {
            if (!homes[i].enabled)
            {
                return false;
            }

        }
        return true;

    }

    private void SetScore(int score)
    {
        this.score = score; 
    }
    private void SetLives(int lives)
    {
        this.lives = lives; 
        livesText.text = lives.ToString();
    }
}
