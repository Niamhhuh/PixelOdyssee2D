using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Dynamic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class AudioManager_Startscreen : MonoBehaviour
{
    public static AudioManager_Startscreen instance {  get; private set; }
    Scene currentScene;

    //Referenz von allen Dingen die abgespielt werden sollen
    private EventInstance TitlescreenMusicInstance;
    private EventInstance DepotMusic;
    
    //---------------------------------------------------------------------------------------------------------
    
    //Check, dass Skript nicht doppelt in Scene
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audiomanager in the scene");
        }
        instance = this;
    }

    //---------------------------------------------------------------------------------------------------------

    //Referenz zu anderen Scripts
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        InitializeTitlescreenMusic(Fmod_Events.instance.TitlescreenMusic);

    }

    //Abspielen von Musik
    private void InitializeTitlescreenMusic(EventReference TitlescreenMusikEventReference)
    {
        TitlescreenMusicInstance = CreateEventInstance(TitlescreenMusikEventReference);
        TitlescreenMusicInstance.start();
    }



    //---------------------------------------------------------------------------------------------------------

    //Funktion, um in anderen Skripts Sound abspielen zu können
    public void PlayOneShot (EventReference Sound, Vector3 worldpos)
    {
        RuntimeManager.PlayOneShot (Sound, worldpos);
    }

    //"CreateInstance" wird ins Leben gerufen
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    //----------------------------------------------------------------------------------------------------------
}
