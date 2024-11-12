using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    public void LoadPong()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadFrogger()
    {
        SceneManager.LoadScene(6);
    }

    public void LoadAstroid()
    {
        SceneManager.LoadScene(7);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadDepot()
    {
        SceneManager.LoadScene(1);
    }
}
