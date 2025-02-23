using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class UiToMouse : MonoBehaviour
{
    private RectTransform rectTransform;
    public Transform player;
    public float playerSpeed = 5f;
    public Canvas canvas;
    public Vector3 targetPosition;
    private bool movePlayer = false;
    
    public bool ClipboardActive;
    public Animator playerAnimator;
    public Animator playerAnimator2;

    private Animator pointerAnimator; // Animator f�r MovePointer
    private Image pointerImage; // Image-Komponente des MovePointers

    private CharacterScript CScript;

    public bool AllowInput;
    public bool LockInteract;
    public bool InTriggerDialogue = false;                      //Input && Interact can't be enabled during triggered dialogue.

    private int lastDirection = 1;
    public Vector3 PermanentmousePosition;

    private PauseMenu PauseScript;

    public bool InCatScene = false;

    private EventInstance FootstepsRosie;  //Sound f�r Footsteps
    private EventInstance FootstepsBebe;

    private DataManager DMReference;

    public WALL WallScript;

    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        CScript = player.GetComponent<CharacterScript>();

        rectTransform = GetComponent<RectTransform>();
        targetPosition = player.position;

        ClipboardActive = false;
        LockInteract = false;


        GameObject movePointer = GameObject.Find("MovePointer");
        pointerAnimator = movePointer.GetComponent<Animator>();
        pointerImage = movePointer.GetComponent<Image>();

        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();

        //FootstepsRosie = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.WalkRosie); //Sound
        //FootstepsBebe = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.WalkBebe);

    }


    public void SetInCatSceneTrue()
    {
        InCatScene = true;
    }

    public void SetInCatSceneFalse()
    {
        InCatScene = false;
    }

    public void DisableInput()
    {
        AllowInput = false;
    }

    public void EnableInput()
    {
        if(ClipboardActive == false && !InTriggerDialogue && !InCatScene)
        {
            AllowInput = true;
        } 
    }

    public void DisableInteract()
    {
        LockInteract = true;
    }

    public void EnableInteract()
    {
        if (!InTriggerDialogue && !InCatScene)
        {
            LockInteract = false;
        }
    }

    void Update()
    {
        PermanentmousePosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0) && AllowInput && !PauseScript.InPause)
        {
            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

            worldPosition.y = -3.5f;
            worldPosition.z = 0f;

            rectTransform.position = worldPosition;

            targetPosition = new Vector3(worldPosition.x, player.position.y, player.position.z);

            movePlayer = true;
            //playerAnimator.SetBool("isWalking", true);
            //playerAnimator2.SetBool("isWalking", true);

            if (targetPosition.x < player.position.x)
            {
                //playerAnimator.SetInteger("Direction", -1); //left
                //playerAnimator2.SetInteger("Direction", -1);
                lastDirection = -1;
            }

            else
            {
                //playerAnimator.SetInteger("Direction", 1); //right
                //playerAnimator2.SetInteger("Direction", 1);
                lastDirection = 1;
            }

            //WAAAAAAAAAAAAAAAAAAAAAAALK ROSIE
            if (playerAnimator != null && DataManager.RosieActive)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetInteger("LastDirection", lastDirection);
                FootstepsRosie.start(); //Sound
            }

            //WAAAAAAAAAAAAAAAAAAAAAAALK BeBe
            if (playerAnimator2 != null && !DataManager.RosieActive)
            {
            playerAnimator2.SetBool("isWalking", true);
            playerAnimator2.SetInteger("LastDirection", lastDirection);
                FootstepsBebe.start(); //Sound
            }

            pointerImage.enabled = true;
            pointerAnimator.Play("UI Pfeil Animation"); // spielt die Animation ab
           
            //-----------------------------------------------------------------------------------------------------------

        }

        if (movePlayer)  //STOOOOOOOOOOOOOOOOOOOOOOOOOOP
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerSpeed * Time.deltaTime);

            if (player.position == targetPosition)
            {
                movePlayer = false;
                
                //STOP ROSIE 
                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("isWalking", false);
                    playerAnimator.SetInteger("LastDirection", lastDirection); //idle
                    FootstepsRosie.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Sound
                }

                // STOP BEBE
                if (playerAnimator2 != null)    
                {
                    playerAnimator2.SetBool("isWalking", false);
                    playerAnimator2.SetInteger("LastDirection", lastDirection);
                    FootstepsBebe.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Sound
                }

                pointerImage.enabled = false;

                //GameObject.Find("MovePointer").GetComponent<Image>().enabled = false;
                //GameObject.Find("MovePointer").GetComponent<Animator>().enabled = false;
            }
        }

    }


    public void DisableWallTrigger()
    {
        if (WallScript != null)
        {
            WallScript.WallTrigger.SetActive(false);
            WallScript.WallClicked = false;
        }
    }


    public void SwitchCharacter()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWalking", false);
        }

        if (playerAnimator2 != null)
        {
            playerAnimator2.SetBool("isWalking", false);
        }

        if (movePlayer)
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetInteger("LastDirection", targetPosition.x < player.position.x ? -1 : 1);
            }

            if (playerAnimator2 != null)
            {
                playerAnimator2.SetBool("isWalking", true);
                playerAnimator2.SetInteger("LastDirection", targetPosition.x < player.position.x ? -1 : 1);
            }
        }
        else
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetInteger("LastDirection", lastDirection);
            }

            if (playerAnimator2 != null)
            {
                playerAnimator2.SetBool("isWalking", false);
                playerAnimator2.SetInteger("LastDirection", lastDirection);
            }
        }
    }

    public void Activate_CallEnableInput()
    {
        StartCoroutine(CallEnableInput());
    }

    public IEnumerator CallEnableInput()
    {
        //print("hi");
        yield return new WaitForEndOfFrame();
        EnableInput();
    }

    public void Activate_CallEnableInteract()
    {
        StartCoroutine(CallEnableInput());
    }


    public IEnumerator CallEnableInteract()
    {
        //print("hi");
        yield return new WaitForEndOfFrame();
        EnableInteract();
    }

    private void UpdateSound()
    {

    }
}
