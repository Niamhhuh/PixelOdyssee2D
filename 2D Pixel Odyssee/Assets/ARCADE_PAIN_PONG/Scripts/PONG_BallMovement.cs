using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public GameObject ActivateTriggerScript;

    [SerializeField] private float initialSpeed = 10;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private Text playerScore;
    [SerializeField] private Text AIScore;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject steuerung;

    //HEALTHBAR ICONS   
    [SerializeField] private GameObject RosieIconDefault;
    [SerializeField] private GameObject RosieIconStrafe;
    [SerializeField] private GameObject SilverIconDefault;
    [SerializeField] private GameObject SilverIconStrafe;
    [SerializeField] private GameObject RahmenDefault;
    [SerializeField] private GameObject RahmenElektro;
    [SerializeField] private GameObject RahmenFeuer;
    [SerializeField] private GameObject RahmenPeitsche;
    [SerializeField] private GameObject HintergrundElektro;
    [SerializeField] private GameObject HintergrundFeuer;
    [SerializeField] private GameObject HintergrundPeitsche;
    [SerializeField] private GameObject HintergrundElektroAI;
    [SerializeField] private GameObject HintergrundFeuerAI;
    [SerializeField] private GameObject HintergrundPeitscheAI;
    [SerializeField] private GameObject HealthSquare1;
    [SerializeField] private GameObject HealthSquare2;
    [SerializeField] private GameObject HealthSquare3;
    [SerializeField] private GameObject HealthSquare1Silver;
    [SerializeField] private GameObject HealthSquare2Silver;
    [SerializeField] private GameObject HealthSquare3Silver;
    [SerializeField] private GameObject RahmenElektroAI;
    [SerializeField] private GameObject RahmenFeuerAI;
    [SerializeField] private GameObject RahmenPeitscheAI;
    [SerializeField] private GameObject RahmenDefaultAI;

    [SerializeField] private Pong_PlayerMovement playerMovement;

    [SerializeField] private GameObject Riss1;
    [SerializeField] private GameObject Riss2;


    private int aiScore;
    private int plScore;

    private Rigidbody2D rb;
    private int hitCounter;

    private bool Scored = false;

    private EventInstance PSBallBounce; //ganz viele Sounds kommen jetzt hier her
    private EventInstance PSElectric;
    private EventInstance PSFire;
    private EventInstance PSWhip;
    private EventInstance PSWin;
    private EventInstance PSLoose;

    private AudioManager script_AudioManager; //Referenz zu "Audiomanager", um Musik anzuhalten

    //-----------------White Screen for peitsche Strafe
    public  GameObject whitescreen_object;
    public Image whitescreen_image;
    private float fadeDuration = 0.5f;

    //_____________________________________________________________________________________
    //-----------------------Set-up below--------------------------------------------------

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        if (steuerung == null) {
            Invoke("StartBall", 2.0f);
        }
        aiScore = 0;
        plScore = 0;

        RosieIconDefault.SetActive(true);
        RosieIconStrafe.SetActive(false);
        SilverIconDefault.SetActive(true);
        SilverIconStrafe.SetActive(false);

        RahmenDefault.SetActive(true);
        RahmenElektro.SetActive(false);
        RahmenFeuer.SetActive(false);
        RahmenPeitsche.SetActive(false);
        RahmenDefaultAI.SetActive(true);
        RahmenElektroAI.SetActive(false);
        RahmenFeuerAI.SetActive(false);
        RahmenPeitscheAI.SetActive(false);

        HintergrundElektro.SetActive(false);
        HintergrundFeuer.SetActive(false);
        HintergrundPeitsche.SetActive(false);
        HintergrundElektroAI.SetActive(false);
        HintergrundFeuerAI.SetActive(false);
        HintergrundPeitscheAI.SetActive(false);

        HealthSquare1.SetActive(true);
        HealthSquare2.SetActive(true);
        HealthSquare3.SetActive(true);
        HealthSquare1Silver.SetActive(true);
        HealthSquare2Silver.SetActive(true);
        HealthSquare3Silver.SetActive(true);

        Riss1.SetActive(false);
        Riss2.SetActive(false);

        PSBallBounce = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSBallBounce); //Sound
        PSElectric = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSElectric);
        PSFire = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSFire);
        PSWhip = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSWhip);
        PSWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSWin);
        PSLoose = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSLoose);

        script_AudioManager = GameObject.Find("AudioManagerMusic").GetComponent<AudioManager>(); //Referenz zu AusiomanagerMusik Component mit "AudioManager" Skript

        PSBallBounce = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSBallBounce); //Sound
        PSElectric = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSElectric);
        PSFire = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSFire);
        PSWhip = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSWhip);
        PSWin = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSWin);
        PSLoose = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.PSLoose);

        script_AudioManager = GameObject.Find("AudioManagerMusic").GetComponent<AudioManager>(); //Referenz zu AusiomanagerMusik Component mit "AudioManager" Skript

        //----------------------------white fade out in peitsche below 
        whitescreen_object = GameObject.Find("C_whitescreen");                //get the gameobject Whitescreen, where another image object should be
        whitescreen_image = whitescreen_object.GetComponent<Image>();       //get the component: image
        whitescreen_image.color = new Color(0, 0, 0, 0);
        whitescreen_object.SetActive(false);                                 //Set it to false, since it only activates during the electro
    }

    private void FixedUpdate() {        //set velocity of ball throughout the game
        if(this != null){
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter));
        }
    }

    private void StartBall() {          //set inital speed
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    private void ResetBall() {          //resets ball to position 0,0 and invokes StartBall()

        Scored = false;
        rb.velocity = new Vector2(0,0);
        transform.position = new Vector2(-0.9f,-0.5f);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }
    //_____________________________________________________________________________________
    //-----------------------Ball movement below-------------------------------------------

    private void PlayerBounce(Transform myObject) {
        hitCounter++;
        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;
        float xDirection, yDirection;

        if(transform.position.x > 0) {
            xDirection = -1;
        }
        else {
            xDirection = 1;
        }
        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if(yDirection == 0) {
            yDirection = 0.25f;
        }
        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease*hitCounter));
    }

    //_____________________________________________________________________________________
    //-----------------------Collisions below----------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "AI") {
            PlayerBounce(collision.transform);

            PSBallBounce.start(); //sound

        }
    }
    //_____________________________________________________________________________________
    //--------------------enter deathzone---------------------------------------------------
    void OnTriggerEnter2D(Collider2D collision) 
    {
        strafen(collision);
        //AFTER the strafbox has hit, adjust health and score and check for WIN
        adjustHealth();
        checkFini();
    }


    private void strafen(Collider2D col) {
        //STRAFEN PLAYER ---------------------------------------------------------------------------

        //ELEKTRO PLAYER-------------------------------------
        if (col.gameObject.CompareTag("StrafboxElektro")) {
            if (aiScore == 0) {
                Debug.Log("StrafboxElektro is hit.");
                HintergrundElektro.SetActive(true);
                HintergrundFeuer.SetActive(false);
                HintergrundPeitsche.SetActive(false);
            }

            else if (aiScore == 1) {
                Debug.Log("StrafboxElektro is hit again.");
                RahmenElektro.SetActive(true);
                RahmenFeuer.SetActive(false);
                RahmenPeitsche.SetActive(false);
                RahmenDefault.SetActive(false);
            }
            ActivateStrafeIcon();
            PSElectric.start(); //sound

            // Toggle reversed controls
            if (playerMovement != null)
            {
                playerMovement.ToggleReverseControls();
            }
            return;
        }

        //FEUER PLAYER-------------------------------------
        else if (col.gameObject.CompareTag("StrafboxFeuer")) {
            if (aiScore == 0) {
                Debug.Log("StrafboxFeuer is hit.");
                HintergrundFeuer.SetActive(true);
                HintergrundElektro.SetActive(false);
                HintergrundPeitsche.SetActive(false);
            }
            else if (aiScore == 1) {
                Debug.Log("StrafboxFeuer is hit again.");
                RahmenFeuer.SetActive(true);
                RahmenElektro.SetActive(false);
                RahmenPeitsche.SetActive(false);
                RahmenDefault.SetActive(false);
            }
            ActivateStrafeIcon();
            PSFire.start(); //sound

            // Increase player speed
            if (playerMovement != null)
            {
                playerMovement.IncreaseSpeed(2.0f); // 200% increase
            }
            return;
        }
        
        //Peitsche PLAYER-------------------------------------
        else if (col.gameObject.CompareTag("StrafboxPeitsche")) {
            if (aiScore == 0) {
                Debug.Log("StrafboxPeitsche is hit.");
                HintergrundPeitsche.SetActive(true);
                HintergrundElektro.SetActive(false);
                HintergrundFeuer.SetActive(false); 
                Riss1.SetActive(true);
            }
            else if (aiScore == 1) {
                Debug.Log("StrafboxPeitsche is hit again.");
                RahmenPeitsche.SetActive(true);
                RahmenElektro.SetActive(false);
                RahmenFeuer.SetActive(false);
                RahmenDefault.SetActive(false);
                Riss2.SetActive(true);
            }
            ActivateStrafeIcon();
            PSWhip.start(); //sound

            //--------white screen
            whitescreen_object.SetActive(true);
            StartCoroutine(FadeOutCoroutine());                                 //then start the fade-out coroutine forthe whitescreen
            return;
        }

        //STRAFE AI----------------------------------------------
        //ELEKTRO AI-------------------------------------
        if (col.gameObject.CompareTag("StrafboxElektroAI")) {
            if (plScore == 0) {
                Debug.Log("You Hit AI StrafboxElektro.");
                HintergrundFeuerAI.SetActive(false);
                HintergrundElektroAI.SetActive(true);
                HintergrundPeitscheAI.SetActive(false);
            }
            else if (plScore == 1) {
                Debug.Log("You Hit AI StrafboxElektro again.");
                RahmenElektroAI.SetActive(true);
                RahmenFeuerAI.SetActive(false);
                RahmenPeitscheAI.SetActive(false);
                RahmenDefaultAI.SetActive(false);
            }
            ActivateAIStrafeIcon();
            PSElectric.start(); //sound
            return;
        }

        //FEUER AI-------------------------------------
        else if (col.gameObject.CompareTag("StrafboxFeuerAI")) {
            if (plScore == 0) {
                Debug.Log("You Hit AI StrafboxFeuer.");
                HintergrundPeitscheAI.SetActive(false);
                HintergrundElektroAI.SetActive(false);
                HintergrundFeuerAI.SetActive(true);
            }
            else if (plScore == 1) {
                Debug.Log("You Hit AI StrafboxFeuer again.");
                RahmenElektroAI.SetActive(false);
                RahmenFeuerAI.SetActive(true);                  
                RahmenPeitscheAI.SetActive(false);
                RahmenDefaultAI.SetActive(false);
            }
            ActivateAIStrafeIcon();
            PSFire.start(); //sound
            return;
        }

        //Peitsche AI-------------------------------------
        else if (col.gameObject.CompareTag("StrafboxPeitscheAI")) {
            if (plScore == 0) {
                Debug.Log("You Hit AI StrafboxPeitsche.");
                HintergrundPeitscheAI.SetActive(true);
                HintergrundElektroAI.SetActive(false);
                HintergrundFeuerAI.SetActive(false);
            }
            else if (plScore == 1) {
                Debug.Log("You Hit AI StrafboxPeitsche again.");
                RahmenElektroAI.SetActive(false);
                RahmenFeuerAI.SetActive(false);                  
                RahmenPeitscheAI.SetActive(true);              
                RahmenDefaultAI.SetActive(false);
            }
            ActivateAIStrafeIcon();
            PSWhip.start(); //sound
            return;
        }
    }

    //_____________________________________________________________________________________
    //--------------------Sprite / Health functions----------------------------------------
    private void adjustHealth() {
        if (transform.position.x > 0 && !Scored) 
        {
            Scored = true;
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            plScore++;

            //Adjust Silver Health Bar
            if (plScore == 1)
            {
                HealthSquare1Silver.SetActive(false);
            }

            if(plScore == 2)
            {
                HealthSquare2Silver.SetActive(false);
            }

            if (plScore == 3)
            {
                HealthSquare3Silver.SetActive(false);
            }            
        }

        else if (transform.position.x < 0 && !Scored) 
        {
            Scored = true;
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
            aiScore++;

            //Adjust player's Health Bar
            if (aiScore == 1)
            {
                HealthSquare3.SetActive(false);
            }
            if (aiScore == 2)
            {
                HealthSquare2.SetActive(false);
            }
            if (aiScore == 3)
            {
                HealthSquare1.SetActive(false);
            }
        }
    }

    private void ActivateStrafeIcon()
    {
        RosieIconDefault.SetActive(false);
        RosieIconStrafe.SetActive(true);

        Invoke("ResetStrafeIcon", 2f);
    }

    private void ResetStrafeIcon()
    {
        RosieIconStrafe.SetActive(false);
        RosieIconDefault.SetActive(true);
    }

    private void ActivateAIStrafeIcon()
    {
        SilverIconDefault.SetActive(false);
        SilverIconStrafe.SetActive(true);

        Invoke("ResetAIStrafeIcon", 2f);
    }

    private void ResetAIStrafeIcon()
    {
        SilverIconStrafe.SetActive(false);
        SilverIconDefault.SetActive(true);
    }
    //_____________________________________________________________________________________
    //--------------------Win state functions----------------------------------------------
    private void checkFini() {  //checks whether the game is finished          
        if(plScore == 3) 
        {
            Scored = false;
            whitescreen_object.SetActive(false);
            Destroy(gameObject);
            winPanel.SetActive(true);

            //--------------------------------------------------------------------------------------------------------------------------------
            //--------Adjust Goals and Triggers---------------------------------------------------------------------------------------------------

            if (ActivateTriggerScript.GetComponent<SimpleActivateTrigger>() != null)
            {
                ActivateTriggerScript.GetComponent<SimpleActivateTrigger>().CallTriggerActivation();
            }

            int i = 0;
            
            
            foreach (DataManager.TriggerableObj Trigger in DataManager.Triggerable_List)            //Activate New Eliza after winning Pain
            {
                if (Trigger.Stored_ID == 69 || Trigger.Stored_ID == 63 || Trigger.Stored_ID == 45)
                {
                    i++;
                    Trigger.Stored_Lock_State = false;

                    if(i >= 3)
                    {
                        break;
                    }
                }
            }

            foreach (DataManager.ActiveGoal Goal in DataManager.ActiveGoal_List)                                        //clear goal
            {
                if(Goal.Stored_ID == 13)
                {
                    Goal.Stored_Completed = true;
                }
            }

            if(GloveScript.GloveProgress < 3)
            {
                GloveScript.CallGlove = true;
                GloveScript.GloveProgress = 3;
            }
            else
            {
                GloveScript.CallGlove = false;
            }

            DataManager.ActiveGoal_List.Add(new DataManager.ActiveGoal { Stored_ID = 16, Stored_Completed = false });      //add Goal

            //--------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------

            script_AudioManager.StopCurrentTheme(); //Musik anhalten
            PSWin.start(); //Sound          
        }
        else if(aiScore == 3) {
            whitescreen_object.SetActive(false);
            Scored = false;
            Destroy(gameObject);    
            losePanel.SetActive(true);

            script_AudioManager.StopCurrentTheme(); //Musik anhalten
            PSLoose.start(); //Sound    
        }
    
        else {
            ResetBall();
        }
    }



    //_____________________________________________________________________________________
    //--------------------White screen-----------------------------------------------------
    private IEnumerator FadeOutCoroutine()               
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1- elapsedTime / fadeDuration);

            whitescreen_image.color = new Color(255, 255, 255, alpha);
            yield return null;
        }

        whitescreen_image.color = new Color(255, 255, 255, 0);
        yield return new WaitForSeconds(0.5f);
        whitescreen_object.SetActive(false);
    }
}
