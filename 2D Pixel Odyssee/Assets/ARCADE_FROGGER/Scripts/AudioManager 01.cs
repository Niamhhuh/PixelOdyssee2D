using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference BGThemeScene1;
    [SerializeField] EventReference BGThemeScene2;

    private EventInstance currentThemeInstance;

    void Start()
    {
        PlayThemeForCurrentScene();
    }

    public void PlayThemeForCurrentScene()
    {
        // Falls ein Theme läuft, stoppen
        if (currentThemeInstance.isValid())
        {
            currentThemeInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentThemeInstance.release();
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Abhängig von der Szene ein neues Theme abspielen
        if (sceneIndex == 0) // Szene 1
        {
            currentThemeInstance = RuntimeManager.CreateInstance(BGThemeScene1);
            currentThemeInstance.start();
        }
        else if (sceneIndex == 1) // Szene 2
        {
            currentThemeInstance = RuntimeManager.CreateInstance(BGThemeScene2);
            currentThemeInstance.start();
        }
    }

    void OnDisable()
    {
        // Falls das Script deaktiviert wird, das Theme ebenfalls stoppen und freigeben
        if (currentThemeInstance.isValid())
        {
            currentThemeInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentThemeInstance.release();
        }
    }
}
