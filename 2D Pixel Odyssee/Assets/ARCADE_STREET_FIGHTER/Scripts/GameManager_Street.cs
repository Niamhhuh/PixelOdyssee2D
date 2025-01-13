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

    public Animator silverAnimator;

    private int lives;

    private int livesEnemy;

    public GameObject gameOverMenuStreet;

    public GameObject gameWonMenuStreet;

    public GameObject gameStartStreet;

    public NoteObject theNO;

    public Text livesTextStreet;

    public Text livesTextEnemyStreet;

    public TriggerAnimation animator;

    public GameObject Hp;

    public GameObject Hp2;

    public GameObject SilverHp1;

    public GameObject SilverHp2;

    public bool allowInput = true;
    // Start is called before the first frame update
    void Awake(){
        animator = GameObject.Find("Round1").GetComponent<TriggerAnimation>();
    }

    void Start()
    {
        instance = this;
        GameObject Rosie = GameObject.Find("Rosie");
        rosieAnimator = Rosie.GetComponent<Animator>();
        GameObject Silver = GameObject.Find("Silver");
        silverAnimator = Silver.GetComponent<Animator>();
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
        animator.PlayScaleAnimationRound1();
        print(animator);
        startPlaying = true;
        theBS.hasStarted = true;
        SetLives(2);
        SetLivesEnemy(27);
        rosieAnimator.Play("Rosie_Idle_Street");
        theMusic.Play();
        
        

        
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
        if(allowInput){
            SetLivesEnemy(livesEnemy - 1);
        }
        if(livesEnemy == 0 && !theNO.round2){
            allowInput = false;
            SetLives(2);
            SetLivesEnemy(27);
            theNO.round2 = true;
            Hp.SetActive(true);
            SilverHp1.SetActive(false);
            
        }
        else if (livesEnemy == 0 && theNO.round2){
            startPlaying = false;
            SilverHp2.SetActive(false);
            rosieAnimator.Play("Rosie_Win_Animation");
            silverAnimator.Play("Silver_Losing_Animation");
            Invoke(nameof(StreetWon), 3f);
        }

    }

    public void NoteMissed(){
    	Debug.Log("NoteMissed");
        Hp.SetActive(false);
        SetLives(lives - 1);
        if(lives == 0){
            startPlaying = false;
            Hp2.SetActive(false);
            rosieAnimator.Play("Rosie_Lose_Animation");
            silverAnimator.Play("Silver_Winning_Animation");
            Invoke(nameof(StreetDeath), 2f);
        }
    }

    public void EndofGame(){

        if(lives > 0){
            NoteHit();
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
