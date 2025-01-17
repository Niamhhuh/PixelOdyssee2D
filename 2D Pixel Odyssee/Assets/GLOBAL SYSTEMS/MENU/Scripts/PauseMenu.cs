using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //_______________________________________________________________________________
    //______________This Manager is existent in every scene__________________________
    //_______________________________________________________________________________

    private DataManager DMReference;

    public GameObject credits = null;

    public GameObject steuerung;        //dieser Block sind die Objekte zur Steuerung UI
    public GameObject allgemein;        //.
    public GameObject spacewar;         //.
    public GameObject frogger;          //.
    public GameObject pong;             //.

    //public GameObject sceneloader;
    //public SpriteRenderer spriteRenderer;
    public GameObject pauseScreen = null;   

    public UiToMouse PointerScript = null;  

    Scene current_scene;    //used in Update() & Neuversuch() --> vergleicht current scene name

    //________________________________________________s_______________________________
    //_______Pausescreen Activator below_____________________________________________

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Pointer") != null)
        {
            PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
            DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        }
    }

    void Update() {  
        current_scene = SceneManager.GetActiveScene();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CallPause();

        } 

        /*if(current_scene.name == "Z_SteuerungPONG" && Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("ARC_Painstation");
        }
        if(current_scene.name == "Z_SteuerungSW" && Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("ARC_Spacewar-MiniGame");
        }*/
    }

    public void CallPause()
    {
        if(current_scene.name != "Z_Start Screen" && current_scene.name != "Z_DemoEnd" && pauseScreen != null && steuerung.activeSelf == false)
        {
            if (PointerScript != null && PointerScript.InTriggerDialogue == false)
            {
                PointerScript.AllowInput = !PointerScript.AllowInput;
                PointerScript.LockInteract = !PointerScript.LockInteract;
            }

            if (pauseScreen.activeSelf)
            {
                Time.timeScale = 1f;
            }

            if (!pauseScreen.activeSelf)
            {
                Time.timeScale = 0f;
            }

            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }


//_______________________________________________________________________________
//_______Buttons for menu to load scene below____________________________________

    public void StartGame() {           //STARTSCREEN --> beginnt im Moment immer beim Tutorial
        if (DataManager.LastRoom == 0) {     //NEUUUUUUU -> 06.12.2024 -> stellt sicher, dass nach reset erste Szene geladen wird
            SceneManager.LoadScene("Z1_Tutorial1");
        }
        else{
            SceneManager.LoadScene(DataManager.LastRoom);
        }
    }

    public void ArcadeReturn() {        //ARCADE GAMES --> schickt den Spieler von den Arcades zurueck in die IRL Welt
    	SceneManager.LoadScene("Z2_Tutorial2");
    }

    public void QuitGame() {            //STARTSCREEN -->Beendet das Spiel komplett
    	Application.Quit();
    }

    public void OpenCredits() {         //STARTSCREEN --> Oeffnet Credits als UI Panel
        credits.SetActive(!credits.activeSelf);
        
    }

    public void Steuerung() {           //START- und PAUSESCREEN --> oeffnet Steuerung als UI screen
        if (pauseScreen != null){
            pauseScreen.SetActive(false);
        }
        
    	steuerung.SetActive(!steuerung.activeSelf);
        allgemein.SetActive(true);
        spacewar.SetActive(false);
        frogger.SetActive(false);
        pong.SetActive(false);   
    }

    public void Return() {              //CREDITS --> schliesst UI
        if(pauseScreen != null) {
            pauseScreen.SetActive(true);
        }
        steuerung.SetActive(false);
        credits.SetActive(false);
       }

    //------------steuerung button below------------------------------------
    public void AllgemeinButton() {
        allgemein.SetActive(true);
        spacewar.SetActive(false);
        frogger.SetActive(false);
        pong.SetActive(false);
    }
    public void SpacewarButton() {
        allgemein.SetActive(false);
        spacewar.SetActive(true);
        frogger.SetActive(false);
        pong.SetActive(false);
    }
    public void FroggerButton() {
        allgemein.SetActive(false);
        spacewar.SetActive(false);
        frogger.SetActive(true);
        pong.SetActive(false);
    }
    public void PongButton() {
        allgemein.SetActive(false);
        spacewar.SetActive(false);
        frogger.SetActive(false);
        pong.SetActive(true);
    }
    //------------steuerung button above------------------------------------

    public void Fortsetzen()
    {          //PAUSESCREEN --> Schliesst den Pausescreen, vll ersetzen durch nochmal esc druecken?
        if (PointerScript != null)
        {
            PointerScript.StartCoroutine(PointerScript.CallEnableInput());
            PointerScript.StartCoroutine(PointerScript.CallEnableInteract());
        }
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void SpielBeenden() {        //PAUSESCREEN --> fuehrt zum Startscreen
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Z_Start Screen");
    }

    public void Retry() {   	        //ARCADE --> laedt spiel neu
        Time.timeScale = 1f;
        SceneManager.LoadScene(current_scene.name);
    }

    /*
//____________________________________________________________________________
//______________________Function interact with scene loader___________________
//-----------------------Doors below------------------------------------------

    private void OnMouseOver(){
        this.spriteRenderer.enabled = true;

        if (Input.GetMouseButtonDown(0) && sceneloader.name == "door_tutorial1"){
            SceneManager.LoadScene("Z_Tutorial2");
        }
        else if (Input.GetMouseButtonDown(0) && sceneloader.name == "door_tutorial2"){
            SceneManager.LoadScene("Z_Tutorial1");
        }
        else if (Input.GetMouseButtonDown(0) && sceneloader.name == "door_tutorial3"){
            SceneManager.LoadScene("Z_DemoEnd");
        }

//-----------------------ArcadeGames below------------------------------------

        else if (Input.GetMouseButtonDown(0) && sceneloader.name == "Mini Space_War"){
            SceneManager.LoadScene("Z_SteuerungSW");
        }
        else if (Input.GetMouseButtonDown(0) && sceneloader.name == "Mini Frogger"){
            SceneManager.LoadScene("ARC_Frogger");
        }
        else if (Input.GetMouseButtonDown(0) && sceneloader.name == "Mini Pong"){
            SceneManager.LoadScene("Z_SteuerungPONG");
        }
    }

    private void OnMouseExit(){
        //arcade games
        this.spriteRenderer.enabled = false;     

        if (this.CompareTag("door")) {
            this.spriteRenderer.enabled = false;
        } 
    }
    */
}
