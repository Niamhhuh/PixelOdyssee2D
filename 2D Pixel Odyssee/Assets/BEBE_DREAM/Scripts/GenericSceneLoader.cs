using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GenericSceneLoader : MonoBehaviour
{
    //public int SpawnPointID;
    public int LoadScene_ID;

    public void LoadScene()
    {
        DataManager.LastRoom = LoadScene_ID;
        SceneManager.LoadScene(LoadScene_ID);
    }
}
