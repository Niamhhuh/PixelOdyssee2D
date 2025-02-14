using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    DataManager DMReference;

    public AdvancedDialogueSO[] conversation;

    private Transform player;
    //private SpriteRenderer speechBubbleRenderer;

    [HideInInspector] public AdvancedDialogueManager advancedDialogueManager;
    [HideInInspector] public GameObject DialogueHolder;                                 //store this gameObject to pass to Advanced Dialogue to check for Trigger
    [HideInInspector] public bool InLockResponse;

    private bool dialogueInitated;
    [HideInInspector] public bool WasClicked;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        advancedDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogueManager>();
        DialogueHolder = gameObject;
        //speechBubbleRenderer = GetComponent<SpriteRenderer>();
        //speechBubbleRenderer.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)      //Change this
    {
        if (collision.gameObject.tag == "Player" && !dialogueInitated && WasClicked)
        {
            //Speech Bubble On
          //  speechBubbleRenderer.enabled=true;

            //Find the player's transform
            //player = collision.gameObject.GetComponent<Transform>();

            // Check to see where the player is, and turn toward them
            /*if (player.position.x > transform.position.x && transform.parent.localScale.x < 0)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && transform.parent.localScale.x < 0)
            {
                Flip();
            }
            */
            dialogueInitated = true;
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && !DMReference.MoveScript.LockInteract && DataManager.ToInteract.Count < 1 && !InLockResponse)
        {
            //print("Initiated");
            advancedDialogueManager.InitiateDialogue(this);
            WasClicked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")                                       // I removed "WasClicked" Condition... this could lead to bugs but for now it seems to improve the interaction flow
        {
            //Speech Bubble Off
            //speechBubbleRenderer.enabled = false;
            
            advancedDialogueManager.TurnOffDialogue();
                                                                                        //advanced Dialogue is nulled!!!!!!!!!
            dialogueInitated = false;

            WasClicked = false;
        }
    }
}
