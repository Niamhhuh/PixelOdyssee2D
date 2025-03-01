using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fades;                                //enables the usage of functions from the script "Fades"

public class PauseMenu : MonoBehaviour
{
    //_______________________________________________________________________________
    //______________This Manager is existent in every scene__________________________
    //_______________________________________________________________________________


    public int ReturntoScene;


    private DataManager DMReference;

    public GameObject credits = null;

    public GameObject steuerung = null;  

    //public GameObject sceneloader;
    //public SpriteRenderer spriteRenderer;
    public GameObject pauseScreen = null;   

    public UiToMouse PointerScript = null;

    public bool InPause = false;

    Scene current_scene;    //used in Update() & Neuversuch() --> vergleicht current scene name
    
    //----------------------STARTSCREEN STUFF---------
    public GameObject c_main;
    public GameObject c_start;
    public GameObject c_neuesspiel;

    //----------------------SPIEL BEENDEN VAR---------
    private GameObject spielBeenden_Fenster;    //sucht nach dem Canvas in jeder Szene --> jedes Canvas heisst gleich
    
    //----------------------FOOTSTEPS STUFF---------
    private UiToMouse script_uitomouse;         //reference to pointer UI for turning off footsteps when pause is on
    private bool pauseActive = false;           //don't allow pause as long as fade is on

    //_______________________________________________________________________________
    //_______Basic functions_________________________________________________________

    private void Awake() {
        if (spielBeenden_Fenster == null) {
            spielBeenden_Fenster = GameObject.FindGameObjectWithTag("FensterQuitGame");         //Spiel beenden Fenster
            if (spielBeenden_Fenster == null) {
                Debug.LogError("Script Pause, Line 56: SpielBeendenFenster not found. In Credits ok, uebrall anders nicht");
                return;  // Stop execution if spiel beenden can't be found
            }   
            spielBeenden_Fenster.SetActive(false);               
        }
        
        //---------------------find script UiMouse in case it is in the scene
        script_uitomouse = FindObjectOfType<UiToMouse>();
        if (script_uitomouse == null) {
            Debug.Log("no Pointer script in this scene");
        }
    }

    private void Start() {
        pauseActive = false;
        if (c_main) {
           c_main.SetActive(true);                                     //make sure everything is set up correctly
            c_start.SetActive(false);
            c_neuesspiel.SetActive(false); 
        } 

        if (GameObject.FindGameObjectWithTag("Pointer") != null) {
            PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
            DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        }
        StartCoroutine(ActivatePause());        //Starts the FadeOut Coroutine from the script "Fades"   ----------------------NEU--------------------
    }

    private IEnumerator ActivatePause() {
        yield return StartCoroutine(Class_Fades.instance.StartFadeOut()); // Wait for fade-out to finish     ----------------------NEU---------------------
        pauseActive = true;
    }

    void Update() {  
        current_scene = SceneManager.GetActiveScene();
        
        if (Input.GetKeyDown(KeyCode.Escape) && pauseActive == true)
        {
            CallPause();

        } 
    }

    //_______________________________________________________________________________
    //_______Pausescreen Activator below_____________________________________________
    public void CallPause() {
        if(current_scene.name != "Z_Start Screen" && current_scene.name != "Z_DemoEnd" && pauseScreen != null && steuerung.activeSelf == false)
        {
            InPause = !InPause;
            if (PointerScript != null && PointerScript.InTriggerDialogue == false)
            {
                PointerScript.AllowInput = !PointerScript.AllowInput;
                PointerScript.LockInteract = !PointerScript.LockInteract;
            }

            if (pauseScreen.activeSelf)
            {
                if (script_uitomouse != null) {
                    if (script_uitomouse.playerAnimator.GetBool("isWalking")) {
                        script_uitomouse.FootstepsRosie.start();
                    }
                    if (script_uitomouse.playerAnimator2.GetBool("isWalking")) {
                        script_uitomouse.FootstepsBebe.start();
                    }
                }
                Time.timeScale = 1f;
            }

            if (!pauseScreen.activeSelf)
            {
                if (script_uitomouse != null) {
                    Debug.Log("Footsteps stopped");
                    script_uitomouse.stopSound();
                }
                
                Time.timeScale = 0f;
            }

            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }


//_______________________________________________________________________________
//_______Buttons below for menu to load scene____________________________________

    public void StartGame() {                                           //STARTSCREEN
        StartCoroutine(StartGameCoroutine());

    }

    private IEnumerator StartGameCoroutine() {
        yield return StartCoroutine(Class_Fades.instance.StartFadeIn()); // Wait for fade-in to finish     ----------------------NEU---------------------
        if (DataManager.LastRoom == 0) {
            SceneManager.LoadScene("Z1_Tutorial1");
        } 
        else {
            SceneManager.LoadScene(DataManager.LastRoom);
        }
    }

    //----------------------STARTSCREEN STUFF---------

    public void MainStart() {
        c_main.SetActive(false);
        c_start.SetActive(true);
        c_neuesspiel.SetActive(false);
    }

    public void MainNeuesSpiel() {
        c_main.SetActive(false);
        c_start.SetActive(false);
        c_neuesspiel.SetActive(true);
    }

    public void MainReturn() {
        if (c_neuesspiel.activeInHierarchy == true) {
            c_main.SetActive(false);
            c_start.SetActive(true);
            c_neuesspiel.SetActive(false);
        }
        else if (c_start.activeInHierarchy == true) {
            c_main.SetActive(true);
            c_start.SetActive(false);
            c_neuesspiel.SetActive(false);
        }
    }

    //----------------------ANDERER STUFF-------------

    public void ArcadeReturn() {        //ARCADE GAMES --> schickt den Spieler von den Arcades zurueck in die IRL Welt
        Time.timeScale = 1f;
        StartCoroutine(returnFromArcade());    	
    }
    private IEnumerator returnFromArcade() {    //works together with the function above
        yield return StartCoroutine(Class_Fades.instance.StartFadeIn()); // Wait for fade-in to finish     ----------------------NEU---------------------
        SceneManager.LoadScene(ReturntoScene);
    }

    public void GameQuit() {            //STARTSCREEN --> Beendet das Spiel komplett
    	Class_Fades.instance.StartFadeIn();        //Starts the FadeIn Coroutine from the script "Fades" ----------------------NEU---------------------
                                            
        Application.Quit();
    }

    public void GameQuitAsk() {         //SPIEL BEENDEN ? --> Oeffnet ein Fenster, was nochmals nachfragt, ob der Spieler das Game beenden moechte  ----------------------NEU---------------------
        if (pauseScreen != null){
            pauseScreen.SetActive(false);
        }

        spielBeenden_Fenster.SetActive(true);
    }

    public void GameResume() {          //SPIEL BEENDEN ? --> Schliesst das zuvor geoeffnete Fenster wieder ----------------------NEU---------------------
        spielBeenden_Fenster.SetActive(false);
        
        if (pauseScreen != null){
            pauseScreen.SetActive(true);
        }
    }

    public void OpenCredits() {         //STARTSCREEN --> Oeffnet Credits als UI Panel
        credits.SetActive(!credits.activeSelf);
        
    }

    public void Steuerung() {           //START- und PAUSESCREEN --> oeffnet Steuerung als UI screen
        if (pauseScreen != null){
            pauseScreen.SetActive(false);
        }

        steuerung.SetActive(true);
    }

    public void Return() {              //CREDITS --> schliesst UI
        if(pauseScreen) {   
            pauseScreen.SetActive(true);
        }
        if (credits) {
            credits.SetActive(false);
        }
        steuerung.SetActive(false);
    }

    public void Fortsetzen()
    {          //PAUSESCREEN --> Schliesst den Pausescreen, vll ersetzen durch nochmal esc druecken?
        if (PointerScript != null)
        {
            PointerScript.StartCoroutine(PointerScript.CallEnableInput());
            PointerScript.StartCoroutine(PointerScript.CallEnableInteract());
        }
        InPause = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void SpielBeenden() {        //PAUSESCREEN --> fuehrt zum Startscreen
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        Time.timeScale = 1f;
        StartCoroutine(returnToStart());    	
    }
    private IEnumerator returnToStart() {    //works together with the function above
        yield return StartCoroutine(Class_Fades.instance.StartFadeIn()); // Wait for fade-in to finish     ----------------------NEU---------------------
        SceneManager.LoadScene("Z_Start Screen");
    }

    public void Retry() {   	        //ARCADE --> laedt spiel neu
        Time.timeScale = 1f;
        SceneManager.LoadScene(current_scene.name);
    }
}
