using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class GameManager_Street : MonoBehaviour
{
	public AudioSource theMusic;

	public bool startPlaying;

    public bool restart;

	public BeatScroller theBS;

    //public UIFade UIFade;

	public static GameManager_Street instance;

    public Animator rosieAnimator;

    public Animator silverAnimator;

    public Animator MissAnim;

    public Animator HitAnim;

    private int lives;

    private int livesEnemy;

    public GameObject gameOverMenuStreet;

    public GameObject gameWonMenuStreet;

    public GameObject gameStartStreet;

    public GameObject VSScreen;

    public NoteObject theNO;

    public Text livesTextStreet;

    public Text livesTextEnemyStreet;

    public TriggerAnimation animator;

    public GameObject Hp;

    public GameObject Hp2;

    public GameObject SilverHp1;

    public GameObject SilverHp2;

    public GameObject Miss;

    public GameObject Hit;

    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f;

    public bool allowInput = true;
    // Start is called before the first frame update

    private EventInstance SFArrowSuccess; //ganz viele Sounds kommen jetzt hier her
    private EventInstance SFArrowFail;
    private EventInstance SFWin;
    private EventInstance SFLoose;
    private EventInstance SFCountdown;
    private EventInstance SFRound1;
    private EventInstance SFPrepareYourself;



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

        SFArrowSuccess = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFArrowSuccess); //Sound
        SFArrowFail = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFArrowFail);
        SFWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFWin);
        SFLoose = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFLoose);
        SFCountdown = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFCountdown);
        SFRound1 = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFRound1);
        SFPrepareYourself = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFPrepareYourself);
    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying){
        	if(Input.GetKeyDown(KeyCode.Return)){
                if(restart == true){
                    StartCoroutine(VS());
                }
        	}
        }
    }
    private IEnumerator VS(){

        
        gameOverMenuStreet.SetActive(false);
        gameStartStreet.SetActive(false);
        StartCoroutine(FadeInOut());
        VSScreen.SetActive(true);
        yield return new WaitForSeconds(2f); 
        NewGame();
    }

    IEnumerator FadeInOut()
    {
        yield return Fade(0, 1); // Fade In
        SFPrepareYourself.start(); //Sound
        yield return new WaitForSeconds(1f);
        yield return Fade(1, 0); // Fade Out
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
    private void NewGame()
    {   
        
        //VSScreen.SetActive(false);
        restart = false;
        animator.PlayScaleAnimationRound1();
        SFCountdown.start(); //Sound
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

            SFLoose.start(); //Sound

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

        SFWin.start(); //Sound

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
        HitAnim.SetTrigger("HitAnim");
        rosieAnimator.SetTrigger("RosieHit");

        SFArrowSuccess.start(); //Sound

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
            silverAnimator.Play("Silver_Crossing_Arms");
            silverAnimator.SetBool("Stage2", true);

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
        MissAnim.SetTrigger("MissAnim");
        rosieAnimator.SetTrigger("RosieMiss");

        SFArrowFail.start(); //Sound

        SetLives(lives - 1);
        if(lives == 0){
            startPlaying = false;
            Hp2.SetActive(false);
            rosieAnimator.SetTrigger("Rosie_Lose_Animation");
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
