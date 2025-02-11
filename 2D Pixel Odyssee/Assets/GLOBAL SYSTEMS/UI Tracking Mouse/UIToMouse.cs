using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Animator pointerAnimator; // Animator für MovePointer
    private Image pointerImage; // Image-Komponente des MovePointers

    public bool AllowInput;
    public bool LockInteract;
    public bool InTriggerDialogue = false;                      //Input && Interact can't be enabled during triggered dialogue.

    private int lastDirection = 1;
    public Vector3 PermanentmousePosition;

    private PauseMenu PauseScript;

    public bool InCatScene = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rectTransform = GetComponent<RectTransform>();
        targetPosition = player.position;

        ClipboardActive = false;
        LockInteract = false;


        GameObject movePointer = GameObject.Find("MovePointer");
        pointerAnimator = movePointer.GetComponent<Animator>();
        pointerImage = movePointer.GetComponent<Image>();

        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();
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

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetInteger("LastDirection", lastDirection);
            }

            if(playerAnimator2 != null)
            {
            playerAnimator2.SetBool("isWalking", true);
            playerAnimator2.SetInteger("LastDirection", lastDirection);
            }

            pointerImage.enabled = true;
            pointerAnimator.Play("UI Pfeil Animation"); // spielt die Animation ab
        }

        if (movePlayer)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerSpeed * Time.deltaTime);

            if (player.position == targetPosition)
            {
                movePlayer = false;
                
                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("isWalking", false);
                    playerAnimator.SetInteger("LastDirection", lastDirection); //idle
                }

                if (playerAnimator2 != null)
                {
                    playerAnimator2.SetBool("isWalking", false);
                    playerAnimator2.SetInteger("LastDirection", lastDirection);
                }

                pointerImage.enabled = false;

                //GameObject.Find("MovePointer").GetComponent<Image>().enabled = false;
                //GameObject.Find("MovePointer").GetComponent<Animator>().enabled = false;
            }
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
}
