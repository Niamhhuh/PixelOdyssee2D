using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedDialogueManager : MonoBehaviour
{
    //NPC DIALOGUE we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    //UI REFERENCES
    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("PortraitImage").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent <TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActivated && Input.GetButtonDown("Interact"))
        {
            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();

            //Continue dialogue
            else
            {
                dialogueText.text = currentConversation.dialogue[stepNum];
                dialogueCanvas.SetActive(true);
                stepNum += 1;
            }
               
        }
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        //the array we are currently stepping through
        currentConversation = npcDialogue.conversation[0];
        Debug.Log("Started converstation" + currentConversation);

        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;
        Debug.Log("Ended converstation. Reset the step to" + stepNum);

        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
    }
}

public enum DialogueActors
{
    Rosie,
    Bebe,
    Silver,
    Random,
    Branch,
};
