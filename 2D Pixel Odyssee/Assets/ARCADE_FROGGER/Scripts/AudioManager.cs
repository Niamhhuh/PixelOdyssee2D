using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private EventReference titelScreenTheme;
    [SerializeField] private EventReference gameSceneTheme;

    private EventInstance currentThemeInstance;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayThemeForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopCurrentTheme();
        PlayThemeForCurrentScene();
    }

    private void PlayThemeForCurrentScene()
    {
        EventReference themeToPlay = default; // Default initialisierung

        if (SceneManager.GetActiveScene().name == "z_Start Screen" || SceneManager.GetActiveScene().name == "ARC_Asteroids" || SceneManager.GetActiveScene().name == "ARC_Frogger" || SceneManager.GetActiveScene().name == "ARC_Painstation") 
        {
            themeToPlay = titelScreenTheme;
        }
        else if (SceneManager.GetActiveScene().name == "Z1_Tutorial1" || SceneManager.GetActiveScene().name == "Z2_Tutorial2"  || SceneManager.GetActiveScene().name == "Z_DemoEnd" ||  SceneManager.GetActiveScene().name == "z_Eliza")
        {
            themeToPlay = gameSceneTheme;
        }

        // Überprüfe, ob ein gültiges Theme gefunden wurde
        if (!themeToPlay.IsNull)
        {
            currentThemeInstance = RuntimeManager.CreateInstance(themeToPlay);
            currentThemeInstance.start();
        }
        else
        {
            Debug.LogWarning("Kein gültiges Theme für die aktuelle Szene gefunden.");
        }
    }

    private void StopCurrentTheme()
    {
        if (currentThemeInstance.isValid())
        {
            currentThemeInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentThemeInstance.release();
            currentThemeInstance = default; // Setze auf null
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopCurrentTheme();
    }
}
