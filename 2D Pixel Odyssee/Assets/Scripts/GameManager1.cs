using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    private Frogger frogger;
    
    private Home[] homes;
    
    private int score;

    private int lives;

    private int time;

    public GameObject gameOverMenu;
    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }

    private void Start()
    {

        NewGame();  
    }

    private void NewGame()
    {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        NewRound();
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

    public void AdvanceRow()
    {
        SetScore(score + 10);
    }

    public void HomeOccupied()
    {
        frogger.gameObject.SetActive(false);

        int bonusPoints = time * 20;
        SetScore(score + bonusPoints + 50);

        if (Cleared())
        {
            SetScore(score + 1000);
            SetLives(lives + 1);
            Invoke (nameof(NewLevel), 1f);
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
    }
}
