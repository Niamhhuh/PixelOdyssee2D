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

    [field: Header("Sfx Frogger")]
    [field: SerializeField] public EventReference FrJump { get; private set; }
    [field: SerializeField] public EventReference FrDeath { get; private set; }
    [field: SerializeField] public EventReference FrGameOver { get; private set; }
    [field: SerializeField] public EventReference FrScore { get; private set; }
    [field: SerializeField] public EventReference FrWin { get; private set; }

    [field: Header("Sfx PainStation")]
    [field: SerializeField] public EventReference PSBallBounce { get; private set; }
    [field: SerializeField] public EventReference PSElectric { get; private set; }
    [field: SerializeField] public EventReference PSFire { get; private set; }
    [field: SerializeField] public EventReference PSWhip { get; private set; }
    [field: SerializeField] public EventReference PSWin { get; private set; }
    [field: SerializeField] public EventReference PSLoose { get; private set; }
    [field: SerializeField] public EventReference CantReachButton { get; private set; }
    [field: SerializeField] public EventReference Lasergun { get; private set; }
    [field: SerializeField] public EventReference FakeHandActivate { get; private set; }
    [field: SerializeField] public EventReference Shaker { get; private set; }
    [field: SerializeField] public EventReference DDRActive { get; private set; }
    [field: SerializeField] public EventReference DDRSnorring { get; private set; }
    [field: SerializeField] public EventReference DDRArrow { get; private set; }


    [field: Header("Sfx Streetfighter")]
    [field: SerializeField] public EventReference SFArrowSuccess { get; private set; }
    [field: SerializeField] public EventReference SFArrowFail { get; private set; }
    [field: SerializeField] public EventReference SFArrowSlow { get; private set; }
    [field: SerializeField] public EventReference SFArrowSlowReverse { get; private set; }
    [field: SerializeField] public EventReference SFArrowSwitch { get; private set; }
    [field: SerializeField] public EventReference SFPlayer1 { get; private set; }
    [field: SerializeField] public EventReference SFPlayer2 { get; private set; }
    [field: SerializeField] public EventReference SFLoose { get; private set; }
    [field: SerializeField] public EventReference SFWin { get; private set; }
    [field: SerializeField] public EventReference SFLoosePlayer1 { get; private set; }
    [field: SerializeField] public EventReference SFLoosePlayer2 { get; private set; }
    [field: SerializeField] public EventReference SFWinPlayer1 { get; private set; }
    [field: SerializeField] public EventReference SFWinPlayer2 { get; private set; }
    [field: SerializeField] public EventReference SFPrepareYourself { get; private set; }
    [field: SerializeField] public EventReference SFRound1 { get; private set; }
    [field: SerializeField] public EventReference SFRound2 { get; private set; }
    [field: SerializeField] public EventReference SFRound3 { get; private set; }
    [field: SerializeField] public EventReference SF3 { get; private set; }
    [field: SerializeField] public EventReference SF2 { get; private set; }
    [field: SerializeField] public EventReference SF1 { get; private set; }
    [field: SerializeField] public EventReference SFFight { get; private set; }
    [field: SerializeField] public EventReference SFCountdown { get; private set; }
    [field: SerializeField] public EventReference SFRoundFinal { get; private set; }
    [field: SerializeField] public EventReference SFButtonClick { get; private set; }
}
