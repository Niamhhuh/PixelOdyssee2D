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
    
    public bool InventoryActive;
    private Animator playerAnimator;

    private Animator pointerAnimator; // Animator für MovePointer
    private Image pointerImage; // Image-Komponente des MovePointers

    public bool AllowInput;
    public bool LockInteract;
    public bool InTriggerDialogue = false;                      //Input && Interact can't be enabled during triggered dialogue.

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = player.position;
        
        InventoryActive = false;
        LockInteract = false;

        playerAnimator = player.GetChild(0).GetComponent<Animator>();

        GameObject movePointer = GameObject.Find("MovePointer");
        pointerAnimator = movePointer.GetComponent<Animator>();
        pointerImage = movePointer.GetComponent<Image>();
    }

    public void DisableInput()
    {
        if(!InTriggerDialogue)
        {
            AllowInput = false;
        }
    }

    public void EnableInput()
    {
        if(InventoryActive == false && !InTriggerDialogue)
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
        LockInteract = false;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && AllowInput)
        {

            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

            worldPosition.y = -3.5f;
            worldPosition.z = 0f;

            rectTransform.position = worldPosition;

            targetPosition = new Vector3(worldPosition.x, player.position.y, player.position.z);

            movePlayer = true;

            playerAnimator.SetBool("isWalking", true);

            if(targetPosition.x < player.position.x)
            {
                playerAnimator.SetInteger("Direction", -1); //left
            }

            else
            {
                playerAnimator.SetInteger("Direction", 1); //right
            }


            pointerImage.enabled = true;
            pointerAnimator.Play("UI Pfeil Animation"); // spielt die Animation ab
            
            
            //GameObject.Find("MovePointer").GetComponent<Image>().enabled = true;
            //GameObject.Find("MovePointer").GetComponent<Animator>().enabled = true;
        }

        if (movePlayer)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerSpeed * Time.deltaTime);

            if (player.position == targetPosition)
            {
                movePlayer = false;

                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetInteger("Direction", 0); //idle

                pointerImage.enabled = false;
                
                //GameObject.Find("MovePointer").GetComponent<Image>().enabled = false;
                //GameObject.Find("MovePointer").GetComponent<Animator>().enabled = false;
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
