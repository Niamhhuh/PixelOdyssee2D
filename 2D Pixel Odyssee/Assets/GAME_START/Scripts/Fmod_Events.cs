using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Fmod_Events : MonoBehaviour
{
    public static Fmod_Events instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found morre than one Fmod Event scripts in this scene");
        }
        instance = this;
    }

    //all music 
    [field: Header ("Titlescreen Musix")]
    [field: SerializeField] public EventReference TitlescreenMusic {  get; private set; }

    //all Sfx
}
