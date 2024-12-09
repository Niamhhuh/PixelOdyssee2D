using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    public void LoadPong()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    public void LoadStreet(){
        Time.timeScale = 1;
        SceneManager.LoadScene(8);
    }
    
    public void LoadFrogger()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(6);
    }

    public void LoadAstroid()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(7);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void LoadDepot()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void LoadArcade()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
}
