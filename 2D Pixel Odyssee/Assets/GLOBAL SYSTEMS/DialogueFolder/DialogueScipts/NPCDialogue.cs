using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AdvancedDialogueSO[] conversation;


    private Transform player;
    //private SpriteRenderer speechBubbleRenderer;

    [HideInInspector] public AdvancedDialogueManager advancedDialogueManager;
    [HideInInspector] public GameObject DialogueHolder;                                 //store this gameObject to pass to Advanced Dialogue to check for Trigger

    private bool dialogueInitated;


    // Start is called before the first frame update
    void Start()
    {
        advancedDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogueManager>();
        DialogueHolder = gameObject;
        //speechBubbleRenderer = GetComponent<SpriteRenderer>();
        //speechBubbleRenderer.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)      //Change this
    {
        if (collision.gameObject.tag == "Player" && !dialogueInitated)
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
        if(Input.GetMouseButtonDown(0) && !dialogueInitated)
        {
            advancedDialogueManager.InitiateDialogue(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Speech Bubble Off
            //speechBubbleRenderer.enabled = false;

            advancedDialogueManager.TurnOffDialogue();
            dialogueInitated = false;
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        transform.parent.localScale = currentScale;
    }
}
