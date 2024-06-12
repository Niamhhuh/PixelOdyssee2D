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

    public GameObject credits;
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

    public void StartGame() {            //beginnt im Moment immer beim Tutorial
    	SceneManager.LoadScene("Z_Tutorial1");
    }

    public void QuitGame() {          //STARTSCREEN -->Beemdet das Spiel komplett
    	Application.Quit();
    }

    public void OpenCredits() {      //STARTSCREEN --> Oeffnet Credits als UI Panel
        credits.SetActive(!credits.activeSelf);
        
    }

    public void Steuerung() {         //START- und PAUSESCREEN --> noch nicht vorhanden
    	SceneManager.LoadScene("Steuerung");
    }

    public void Fortsetzen() {       //PAUSESCREEN --> Schliesst den, vll ersetzen durch nochmal esc druecken?
    	pauseScreen.SetActive(!pauseScreen.activeSelf);
    }

    public void SpielBeenden() {      //PAUSESCREEN --> fuehrt zum Startscreen
    	SceneManager.LoadScene("Startscreen");
    }

//____________________________________________________________________________
//______________________Function interact with scene loader___________________
//-----------------------Doors below------------------------------------------

    private void OnMouseOver(){
        this.spriteRenderer.enabled = true;

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
        this.spriteRenderer.enabled = false;
    }
}
