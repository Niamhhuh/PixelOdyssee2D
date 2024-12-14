using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager_Street : MonoBehaviour
{
	public AudioSource theMusic;

	public bool startPlaying;

    public bool restart;

	public BeatScroller theBS;

	public static GameManager_Street instance;

    public Animator rosieAnimator;

    private int lives;

    private int livesEnemy;

    public GameObject gameOverMenuStreet;

    public GameObject gameWonMenuStreet;

    public GameObject gameStartStreet;

    public Text livesTextStreet;

    public Text livesTextEnemyStreet;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GameObject Rosie = GameObject.Find("Rosie");
        rosieAnimator = Rosie.GetComponent<Animator>();
        gameStartStreet.SetActive(true);
        restart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying){
        	if(Input.GetKeyDown(KeyCode.Return)){
                if(restart == true){
                    NewGame();
                }
        	}
        }
    }

    private void NewGame()
    {   
        gameOverMenuStreet.SetActive(false);
        gameStartStreet.SetActive(false);
        restart = false;
        startPlaying = true;
        theBS.hasStarted = true;
        SetLives(1);
        SetLivesEnemy(27);
        theMusic.Play();
        rosieAnimator.Play("Rosie_Idle_Street");
        
        //StartScreen();
        
    }

    public void StreetDeath(){
            startPlaying = false;
            theBS.hasStarted = false;
            gameOverMenuStreet.SetActive(true);
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

    private void StreetWon(){

        gameWonMenuStreet.gameObject.SetActive(true);
        StopAllCoroutines();
        startPlaying = false;
        theBS.hasStarted = false;
        StopAllCoroutines();
        //StartCoroutine(BackToHub());
    }

    /*private IEnumerator BackToHub(){

        bool BackToHub = false;
        while (!BackToHub)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                BackToHub = true;
            }

            yield return null;
        }
        SceneManager.LoadScene("Z_TutorialRoom2");
    }*/

    public void NoteHit(){
    	Debug.Log("NoteHit");
        SetLivesEnemy(livesEnemy - 1);
        if(livesEnemy == 0){
            startPlaying = false;
            Invoke(nameof(StreetWon), 1f);
        }

    }

    public void NoteMissed(){
    	Debug.Log("NoteMissed");
        SetLives(lives - 1);
        if(lives == 0){
            startPlaying = false;
            Invoke(nameof(StreetDeath), 1f);
        }
    }
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesTextEnemyStreet.text = lives.ToString();
    }
    private void SetLivesEnemy(int livesEnemy)
    {
        this.livesEnemy = livesEnemy;
        livesTextStreet.text = livesEnemy.ToString();
    }
}
