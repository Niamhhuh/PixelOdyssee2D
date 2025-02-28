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
    [SerializeField] private EventReference AliceAmbience;
    [SerializeField] private EventReference AstreoidsTheme;
    [SerializeField] private EventReference FroggerTheme;
    [SerializeField] private EventReference PainStationTheme;
    [SerializeField] private EventReference StreetfighterTheme;
    [SerializeField] private EventReference Credits;

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

    public void PlayThemeForCurrentScene()
    {
        EventReference themeToPlay = default; // Default initialisierung

        if (SceneManager.GetActiveScene().name == "z_Start Screen")
        {
            themeToPlay = titelScreenTheme;
        }
        else if (SceneManager.GetActiveScene().name == "Z1_Tutorial1" || SceneManager.GetActiveScene().name == "Z2_Tutorial2"  || SceneManager.GetActiveScene().name == "Z_DemoEnd" ||  SceneManager.GetActiveScene().name == "Z3_Eingang" || SceneManager.GetActiveScene().name == "Z4_Alice" || SceneManager.GetActiveScene().name == "Z5_Arcade2" || SceneManager.GetActiveScene().name == "Z6_Sinne" || SceneManager.GetActiveScene().name == "Z3_Eingang" || SceneManager.GetActiveScene().name == "Z7_Indie" || SceneManager.GetActiveScene().name == "Z8_Boss")
        {
            themeToPlay = gameSceneTheme;
        }

        else if (SceneManager.GetActiveScene().name == "z_Eliza")
        {
            themeToPlay = AliceAmbience;
        }

        else if (SceneManager.GetActiveScene().name == "ARC_Asteroids")
        {
            themeToPlay = AstreoidsTheme;
        }

        else if (SceneManager.GetActiveScene().name == "ARC_Frogger")
        {
            themeToPlay = FroggerTheme;
        }

        else if (SceneManager.GetActiveScene().name == "ARC_Painstation")
        {
            themeToPlay = PainStationTheme;
        }

        else if (SceneManager.GetActiveScene().name == "ARC_Streetfighter")
        {
            themeToPlay = StreetfighterTheme;
        }

        else if (SceneManager.GetActiveScene().name == "Z9_Credits")
        {
            themeToPlay = Credits;
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

    public void StopCurrentTheme()
    {
        if (currentThemeInstance.isValid())
        {
            currentThemeInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentThemeInstance.release();
            currentThemeInstance = default; // Setze auf null
        }
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopCurrentTheme();
    }
}
