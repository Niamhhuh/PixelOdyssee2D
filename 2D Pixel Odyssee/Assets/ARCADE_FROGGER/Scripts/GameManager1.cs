using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using Fades;

public class GameManager1 : MonoBehaviour
{
    public Frogger frogger;
    
    private Home[] homes;

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

    public PauseMenu script_pause;              //NEU --> to turn off pause when steuerung is active
    
//___________________________________________________________________________________________________
//------------------------Standard functions---------------------------------------------------------
    private void Awake() {    
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }

    private void Start() {
        frogger.gameObject.SetActive(false);
        gameStart.SetActive(true);
        StartCoroutine(StartSteuerung());

        FrScore = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrScore); //Sound
        FrWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrWin);
        FrDeath = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrDeath);
        FrLoose = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.FrGameOver);

        script_AudioManager = GameObject.Find("AudioManagerMusic").GetComponent<AudioManager>(); //Referenz zu AusiomanagerMusik Component mit "AudioManager" Skript

        StartCoroutine(Class_Fades.instance.StartFadeOut());        //makes sure the fade in happens because below, we turn off the pause script
        script_pause.enabled = false;                               //turn off pause script so it does not overlap with the beginning steuerung
    }

//___________________________________________________________________________________________________
//------------------------Start Game------------------------------------------------------
    private void NewGame() {               
        gameOverMenu.SetActive(false);
        frogger.gameObject.SetActive(true);
        gameStart.SetActive(false);
        script_pause.enabled = true;  

        script_AudioManager.StopCurrentTheme();     //Musik anhalten

        SetLives(3);
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        Respawn(); 
    }

//___________________________________________________________________________________________________
//------------------------WIN LOOSE SCREEN-----------------------------------------------------------
    private void NewLevel() {                       //-------------------WIN
        frogger.gameObject.SetActive(false);
        gameWonMenu.gameObject.SetActive(true);


        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------

        foreach ( DataManager.TriggerableObj Trigger in DataManager.Triggerable_List)            //Activate New Eliza after winning Frogger
        {
            if(Trigger.Stored_ID == 44)
            {
                Trigger.Stored_Lock_State = false;
                break;
            }
        }


        foreach (DataManager.ActiveGoal Goal in DataManager.ActiveGoal_List)
        {
            if (Goal.Stored_ID == 8)
            {
                Goal.Stored_Completed = true;
            }
        }

        DataManager.ActiveGoal_List.Add(new DataManager.ActiveGoal { Stored_ID = 12, Stored_Completed = false });      //add Goal

        if(GloveScript.GloveProgress < 2)
        {
            GloveScript.CallGlove = true;
            GloveScript.GloveProgress = 2;
        }
        else
        {
            GloveScript.CallGlove = false;
        }

        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------


        StopAllCoroutines();
    }

    private void GameOver() {                       //--------------------LOOSE
        frogger.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);

        script_AudioManager.StopCurrentTheme();     //Musik anhalten
        FrLoose.start();                            //Sound

        StopAllCoroutines();
    }
//___________________________________________________________________________________________________
//------------------------Die, death or respawn------------------------------------------------------
    public void Died() {                            //gets called by script Frogger
        Debug.Log("died");
        if (frogger.gameObject.activeInHierarchy == true) {
            SetLives(lives - 1);    

            if (lives > 0) {
                FrDeath.start(); //Sound
                script_AudioManager.StopCurrentTheme(); //Musik anhalten

                Invoke(nameof(Respawn), 1f);            //respawn if we have lives left
            }
            else {
                Invoke(nameof(GameOver), 1f);           //game over if we have no lives left
            }
        }
    }

    private void Respawn() {
        frogger.noMove = false; 
        frogger.Respawn();
        Debug.Log("Respawn");

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

//___________________________________________________________________________________________________
//------------------------etc stuff------------------------------------------------------------------
    private IEnumerator Timer(int duration) {       //controls the timer
        time = duration;
        script_AudioManager.PlayThemeForCurrentScene(); //Musik abspielen
        while (time > -1) { 
            yield return new WaitForSeconds(1);
            timerText.text = time.ToString();
            time --;
        }
        if (!Cleared() && lives > 0) {
            frogger.Death();
        }
    }

    private IEnumerator StartSteuerung() {          //controls the steuerung panel at the beginning
        bool startScreen = false;  
        while (!startScreen) { 
            if (Input.GetKeyDown(KeyCode.Return)) { 
                startScreen = true;
            }
            yield return null;  
        }
        NewGame();  
    }


//___________________________________________________________________________________________________
//------------------------Frogger home stuff---------------------------------------------------------
    public void HomeOccupied() {
        frogger.gameObject.SetActive(false);

        script_AudioManager.StopCurrentTheme();     //Musik anhalten
        FrScore.start();                            //Sound

        if (Cleared()) {
            StopAllCoroutines();
            Invoke (nameof(NewLevel), 1f);          //invoke WIN
            script_AudioManager.StopCurrentTheme(); //Musik anhalten
            FrWin.start();                          //Sound
        }
        else {
            Invoke (nameof(Respawn), 1f);           //invoke RESPAWN
        }
    }

    private bool Cleared() {
        for (int i = 0; i<homes.Length; i++) {
            if (!homes[i].enabled) {
                return false;
            }
        }
        return true;
    }

    private void SetLives(int lives) {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}