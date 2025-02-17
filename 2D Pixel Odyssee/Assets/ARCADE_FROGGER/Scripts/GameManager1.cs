using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class GameManager1 : MonoBehaviour
{
    private Frogger frogger;
    
    private Home[] homes;
    
    private int score;

    private int lives;

    private int time;

    public GameObject gameOverMenu;

    public GameObject gameWonMenu;

    public GameObject gameStart;

    public Text livesText;

    public Text timerText;


    SoundManager soundManager;

    private EventInstance FrScore; //ganz viele Sounds kommen jetzt hier her
    private EventInstance FrWin;
    private EventInstance FrDeath;
    private EventInstance FrLoose;

    private AudioManager script_AudioManager; //Referenz zu "Audiomanager", um Musik anzuhalten

    private void Awake()
    {
        
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void Start()
    {
        Play();
        //NewGame();
        soundManager.PlayMusic(soundManager.background);

        FrScore = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrScore); //Sound
        FrWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrWin);
        FrDeath = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrDeath);
        FrLoose = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrGameOver);

        script_AudioManager = GameObject.Find("AudioManagerMusic").GetComponent<AudioManager>(); //Referenz zu AusiomanagerMusik Component mit "AudioManager" Skript

    }

    private void NewGame()
    {
        gameOverMenu.SetActive(false);
        frogger.gameObject.SetActive(true);
        gameStart.SetActive(false);
        soundManager.PlayMusic(soundManager.background);

        script_AudioManager.StopCurrentTheme(); //Musik anhalten

        SetScore(0);
        SetLives(3);
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        //StartScreen();
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
        DataManager.FroggerCleared = true;

        StopAllCoroutines();
        StartCoroutine(BackToHub());
    }

    private void NewRound() 
    {  
        Respawn();
    }

    private void Respawn()
    {
        frogger.noMove = false; 
        frogger.Respawn();

        script_AudioManager.PlayThemeForCurrentScene(); //Musik abspielen

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        time = duration;

        while (time > 0) 
        { 
            yield return new WaitForSeconds(1);
            timerText.text = time.ToString();
            time --;
        }

        frogger.Death();
    }

    public void Died()
    {
        SetLives(lives - 1);    

        if (lives > 0)
        {
            FrDeath.start(); //Sound
            script_AudioManager.StopCurrentTheme(); //Musik anhalten

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

        script_AudioManager.StopCurrentTheme(); //Musik anhalten
        FrLoose.start(); //Sound

        StopAllCoroutines();
        StartCoroutine(PlayAgain());

    }

    private void Play()
    {
        frogger.gameObject.SetActive(false);
        gameStart.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(StartScreen());
    }
    private IEnumerator StartScreen()
    {
        bool startScreen = false;  
        while (!startScreen) 
        { 
            if (Input.GetKeyDown(KeyCode.Return)) 
            
            { 
                startScreen = true;
            }

            yield return null;  
        }
        NewGame();  
        
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

        script_AudioManager.StopCurrentTheme(); //Musik anhalten
        FrScore.start(); //Sound

        if (Cleared())
        {
            SetScore(score + 1000);
            SetLives(lives + 1);
            Invoke (nameof(NewLevel), 1f);
            soundManager.StopMusic(soundManager.background);
            soundManager.PlaySfx(soundManager.win);

            script_AudioManager.StopCurrentTheme(); //Musik anhalten
            FrWin.start(); //Sound
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
