using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
//_______________________________________________________________________________
//______________This Manager is existent in every scene and______________________
//_______________is not destroyed upon loading a new scene_______________________
//_______________________________________________________________________________

    public GameObject credits = null;

    public GameObject steuerung;
    public GameObject allgemein;
    public GameObject spacewar;
    public GameObject frogger;
    public GameObject pong;

    public GameObject sceneloader;
    public SpriteRenderer spriteRenderer;
    public GameObject pauseScreen;

//_______________________________________________________________________________
//_______Pausescreen Activator below_____________________________________________

   /* void Start() {
        pauseScreen = GameObject.Find("Pausescreen");
    }*/

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        } 
    }

//_______________________________________________________________________________
//_______Buttons for menu to load scene below____________________________________

    public void StartGame() {           //beginnt im Moment immer beim Tutorial
    	SceneManager.LoadScene("Z_Tutorial1");
    }

    public void ArcadeReturn() {        //schickt den Spieler von den Arcades zurueck in die IRL Welt
    	SceneManager.LoadScene("Z_Tutorial2");
    }

    public void QuitGame() {            //STARTSCREEN -->Beemdet das Spiel komplett
    	Application.Quit();
    }

    public void OpenCredits() {         //STARTSCREEN --> Oeffnet Credits als UI Panel
        credits.SetActive(!credits.activeSelf);
        
    }

    public void Steuerung() {           //START- und PAUSESCREEN --> oeffnet Steuerung als UI screen
    	steuerung.SetActive(!steuerung.activeSelf);
        allgemein.SetActive(true);
        spacewar.SetActive(false);
        frogger.SetActive(false);
        pong.SetActive(false);
    }

    public void Return() {
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

    void Fortsetzen() {       //PAUSESCREEN --> Schliesst den, vll ersetzen durch nochmal esc druecken?
    	pauseScreen.SetActive(false);
    }

    void SpielBeenden() {      //PAUSESCREEN --> fuehrt zum Startscreen
    	SceneManager.LoadScene("Startscreen");
    }

//____________________________________________________________________________
//______________________Function interact with scene loader___________________
//-----------------------Doors below------------------------------------------

    private void OnMouseOver(){
        this.spriteRenderer.enabled = true;

        Color alpha = this.GetComponent<SpriteRenderer>().color;
        alpha.a = 255f;
        this.GetComponent<SpriteRenderer>().color = alpha;

        if (Input.GetMouseButtonDown(1) && sceneloader.name == "door_tutorial1"){
            SceneManager.LoadScene("Z_Tutorial2");
        }
        else if (Input.GetMouseButtonDown(1) && sceneloader.name == "door_tutorial2"){
            SceneManager.LoadScene("Z_Tutorial1");
        }

//-----------------------ArcadeGames below------------------------------------

        else if (Input.GetMouseButtonDown(1) && sceneloader.name == "Mini Space_War"){
            SceneManager.LoadScene("Spacewar-MiniGame");
        }
        else if (Input.GetMouseButtonDown(1) && sceneloader.name == "Mini Frogger"){
            SceneManager.LoadScene("Frogger");
        }
        else if (Input.GetMouseButtonDown(1) && sceneloader.name == "Mini Pong"){
            SceneManager.LoadScene("ARC_Painstation");
        }
    }

    private void OnMouseExit(){
        //arcade games
        Color alpha = this.GetComponent<SpriteRenderer>().color;
        alpha.a = 150f;
        this.GetComponent<SpriteRenderer>().color = alpha;

        if (this.CompareTag("door")) {
            this.spriteRenderer.enabled = false;
        } 
    }
}
