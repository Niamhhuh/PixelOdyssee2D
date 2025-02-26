using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fades;

public class SceneLoaderScript : MonoBehaviour
{
    private string sceneName;

    //-----------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------Enumerator Stuff------------------------------------------------------------------------
 
    private IEnumerator LoadNewScene(string name) {
        yield return StartCoroutine(Class_Fades.instance.StartFadeIn()); // Wait for fade-in to finish     ----------------------NEU---------------------
        SceneManager.LoadScene(name);
    }
 
    //-----------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------Buttons Stuff---------------------------------------------------------------------------
 
    public void LoadPong() {
        sceneName = "ARC_Painstation";
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void LoadStreet() {
        sceneName = "ARC_Streetfighter";
        StartCoroutine(LoadNewScene(sceneName));
    }
    
    public void LoadFrogger() {
        sceneName = "ARC_Frogger";
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void LoadAstroid() {
        sceneName = "ARC_Asteroids";
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void LoadMenu()
    {
        sceneName = "z_Start Screen";
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void LoadDepot()
    {
        sceneName = "Z1_Tutorial1";
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void LoadArcade()
    {
        sceneName = "Z2_Tutorial2";
        StartCoroutine(LoadNewScene(sceneName));
    }
}
