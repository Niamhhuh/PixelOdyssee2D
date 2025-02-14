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

    //all music --> wird nicht verwendet, weil es ein extra Skript für Musik gibt mit dem Namen "AudioManager"
    [field: Header ("Titlescreen Music")]
    [field: SerializeField] public EventReference TitlescreenMusic {  get; private set; }
    [field: SerializeField] public EventReference DepotMusic { get; private set; }

    //all Sfx

    [field: Header("Player Sfx")]
    [field: SerializeField] public EventReference WalkRosie { get; private set; }
    [field: SerializeField] public EventReference WalkBebe { get; private set; }

    [field: Header("Sfx HubWorld")]
    [field: SerializeField] public EventReference InventoryItem { get; private set; }

    [field: Header("Sfx Asteroids")]
    [field: SerializeField] public EventReference AstShot { get; private set; }
    [field: SerializeField] public EventReference AstHitAsteroid { get; private set; }
    [field: SerializeField] public EventReference AstHitPlayer { get; private set; }
    [field: SerializeField] public EventReference AstTeleport { get; private set; }
    [field: SerializeField] public EventReference AstBoost { get; private set; }
    [field: SerializeField] public EventReference AstWin { get; private set; }
    [field: SerializeField] public EventReference AstGameOver { get; private set; }
}
