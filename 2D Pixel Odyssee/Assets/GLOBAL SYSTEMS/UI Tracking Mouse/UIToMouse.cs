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
    private Animator playerAnimator;
    private Animator playerAnimator2;

    private Animator pointerAnimator; // Animator für MovePointer
    private Image pointerImage; // Image-Komponente des MovePointers

    public bool AllowInput;
    public bool LockInteract;
    public bool InTriggerDialogue = false;                      //Input && Interact can't be enabled during triggered dialogue.

    private int lastDirection = 1;
    public Vector3 PermanentmousePosition;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rectTransform = GetComponent<RectTransform>();
        targetPosition = player.position;

        ClipboardActive = false;
        LockInteract = false;

        playerAnimator = GameObject.FindGameObjectWithTag("Rosie").GetComponent<Animator>();
        playerAnimator2 = GameObject.FindGameObjectWithTag("Bebe").GetComponent<Animator>();


        GameObject movePointer = GameObject.Find("MovePointer");
        pointerAnimator = movePointer.GetComponent<Animator>();
        pointerImage = movePointer.GetComponent<Image>();
    }

    public void DisableInput()
    {
        AllowInput = false;
    }

    public void EnableInput()
    {
        if(ClipboardActive == false && !InTriggerDialogue)
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
        if (!InTriggerDialogue)
        {
            LockInteract = false;
        }
    }

    void Update()
    {
        PermanentmousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && AllowInput)
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


            playerAnimator.SetBool("isWalking", true);
            playerAnimator.SetInteger("LastDirection", lastDirection);
            playerAnimator2.SetBool("isWalking", true);
            playerAnimator2.SetInteger("LastDirection", lastDirection);

            pointerImage.enabled = true;
            pointerAnimator.Play("UI Pfeil Animation"); // spielt die Animation ab
        }

        if (movePlayer)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerSpeed * Time.deltaTime);

            if (player.position == targetPosition)
            {
                movePlayer = false;

                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetInteger("LastDirection", lastDirection); //idle

                playerAnimator2.SetBool("isWalking", false);
                playerAnimator2.SetInteger("LastDirection", lastDirection);

                pointerImage.enabled = false;

                //GameObject.Find("MovePointer").GetComponent<Image>().enabled = false;
                //GameObject.Find("MovePointer").GetComponent<Animator>().enabled = false;
            }
        }

    }

    public void SwitchCharacter(Transform newPlayer)
    {
        playerAnimator.SetBool("isWalking", false);

        playerAnimator2.SetBool("isWalking", false);


        player = newPlayer;

        if (movePlayer)
        {
            playerAnimator.SetBool("isWalking", true);
            playerAnimator2.SetBool("isWalking", true);


            playerAnimator.SetInteger("LastDirection", targetPosition.x < player.position.x ? -1 : 1);
            playerAnimator2.SetInteger("LastDirection", targetPosition.x < player.position.x ? -1 : 1);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetInteger("LastDirection", lastDirection);

            playerAnimator2.SetBool("isWalking", false);
            playerAnimator2.SetInteger("LastDirection", lastDirection);
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
