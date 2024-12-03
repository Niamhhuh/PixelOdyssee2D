using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedDialogueManager : MonoBehaviour
{

    private DataManager DMReference;
    //NPC DIALOGUE we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum = 0;
    private bool dialogueActivated;

    //UI REFERENCES
    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;

    //BUTTON REFERENCES
    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    private GameObject ContinueButton;          //Store Button which is used to continue Dialogue

    //TYPEWRITER EFFECT
    [SerializeField]
    private float typingSpeed = 0.02f;
    public Coroutine typeWriterRoutine;
    public bool canContinueText = true;

    NPCDialogue CurrentNPC = null;

    private bool StopTypeWriter;
    private bool WriterIsRunning;

    static int A;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        //FIND BUTTONS
        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionPanel");
        optionsPanel.SetActive(false);

        ContinueButton = GameObject.FindGameObjectWithTag("ContinueDialogueButton");

        //FIND THE TMP TEXT ON THE BUTTONS
        optionButtonText = new TMP_Text[optionButton.Length];
        for (int i = 0; i < optionButtonText.Length; i++)
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();

        //Turn off the buttons to start
        for(int i = 0;i < optionButtonText.Length; i++)
            optionButton[i].SetActive(false);


        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("PortraitImage").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent <TMP_Text>();

        dialogueCanvas.SetActive(false);

    }

    // Update is called once per frame
    public void ContinueDialogue()
    {
        if (dialogueActivated && canContinueText)
        {
            //Cancel dialogue if there are no lines of dialogue remaining
            if (currentConversation != null && stepNum >= currentConversation.actors.Length)
            {
                if (CurrentNPC.DialogueHolder.GetComponent<ActivateTrigger>() != null) 
                {
                    CurrentNPC.DialogueHolder.GetComponent<ActivateTrigger>().CallTriggerActivation(3); // Call Trigger when Dialogue has been concluded
                } 

                EndTriggerDialogue();

                EndEventDialogue();

                TurnOffDialogue(gameObject.name);
            }

            //Continue dialogue1
            else
                if (currentConversation != null)
                PlayDialogue();
        }
    }

    void EndTriggerDialogue()
    {
        if (CurrentNPC.DialogueHolder.GetComponent<Triggerable>() != null && CurrentNPC.DialogueHolder.GetComponent<Triggerable>().ForceDialogue == true)
        {
            DMReference.MoveScript.InTriggerDialogue = false;
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInput());            //Enable Inpput Again
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInteract());         //Enable Interact Again
            CurrentNPC.DialogueHolder.GetComponent<Triggerable>().RemoveTrigger();
        }
    }

    void EndEventDialogue()
    {
        if (CurrentNPC.DialogueHolder.GetComponent<EventSource>() != null && CurrentNPC.DialogueHolder.GetComponent<EventSource>().Talk_Event == true)
        {
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInput());            //Enable Inpput Again
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInteract());         //Enable Interact Again
            CurrentNPC.DialogueHolder.GetComponent<EventSource>().EventInteract();
        }
    }


    void PlayDialogue()
    {
        Debug.Assert(currentConversation.actors.Length > stepNum, "stepNum out of range of actors. Did you forgot to assign it?");

        //If it's a random NPC
        if (currentConversation.actors[stepNum] == DialogueActors.Random)
            SetActorInfo(false);

        //If it's a recurring Character
        else
            SetActorInfo(true);

        //Display Dialogue
        
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;                                                  //---------------------------------
        ContinueButton.SetActive(true);
        //If there is a branch...
        if (currentConversation.actors[stepNum] == DialogueActors.Branch)
        {
            ContinueButton.SetActive(false);                                                //Deactivate the Continue Dialogue Button when an Option Branch is triggered
           
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInput());            //Enable Inpput Again
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInteract());         //Enable Interact Again                                                                               //Thist looks weird look out for it latr
            
            for (int i = 0;i < currentConversation.optionText.Length; i++)
            {
                if (currentConversation.optionText[i] == null)
                    optionButton[i].SetActive(false);
                else
                {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButton[i].SetActive(true);
                }

                //Set the first button to be auto-selected
                //optionButton[0].GetComponent<Button>().Select();
            }
        }

        //Keep routine from running multiple times at the same time
        if(typeWriterRoutine != null)
        {
            StopCoroutine(typeWriterRoutine);
        }


        if (stepNum < currentConversation.dialogue.Length)
        {
            typeWriterRoutine = StartCoroutine(TypewriterEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        }
        else
            optionsPanel.SetActive(true);
       
        dialogueCanvas.SetActive(true);
        stepNum += 1;

    }

    void SetActorInfo(bool recurringCharacter)
    {
        if(recurringCharacter)
        {
            for (int i = 0; i < actorSO.Length; i++)
            {
                if (actorSO[i].actorName == currentConversation.actors[stepNum].ToString())
                {
                    currentSpeaker = actorSO[i].actorName;
                    currentPortrait = actorSO[i].actorPortrait;
                }
            }
        }
        else
        {
            currentSpeaker = currentConversation.randomActorName;
            currentPortrait = currentConversation.randomActorPortait;                           //---------------------------------
        }
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionButton)
            button.SetActive(false);

        if (optionNum == 0) 
            currentConversation = currentConversation.option0;
        if (optionNum == 1)
            currentConversation = currentConversation.option1; 
        if (optionNum == 2)
            currentConversation = currentConversation.option2;
        if (optionNum == 3)
            currentConversation = currentConversation.option3;

        stepNum = 0;
        PlayDialogue();
    }

    /*
                 if (Input.GetButtonDown("Interact"))
            {
                dialogueText.text = line;
                break;
            }
     */

    public void SkipDialogue()
    {
        StopTypeWriter = true;

        if(stepNum > 0)
        {
            dialogueText.text = currentConversation.dialogue[stepNum - 1];
        }
    }

        public IEnumerator TypewriterEffect(string line)
    {
        WriterIsRunning = true;
        dialogueText.text = "";
        canContinueText = false;
        bool addingRichTextTag = false;
        yield return new WaitForSeconds(.3f);
        foreach(char letter in line.ToCharArray())
        {
            if (StopTypeWriter)
            {
                canContinueText = true;
                WriterIsRunning = false;
                StopTypeWriter = false;
                break;
            }

            //Check to see if we are working with rich text tags
            if (letter == '<' || addingRichTextTag)
            {
                addingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                    addingRichTextTag = false;
            }

            //if NOT using rich text tags
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }
        WriterIsRunning = false;
        canContinueText = true;
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        CurrentNPC = npcDialogue;
        if (WriterIsRunning)
        {
            StopTypeWriter = true;
            dialogueText.text = "";
        }

        //the array we are currently stepping through
        if (DMReference.CurrentCharacter.RosieActive == true && npcDialogue.conversation.Length > 0 && npcDialogue.conversation[0] != null)
        {
            currentConversation = npcDialogue.conversation[0];
        }

        else                                                                  //add a selector to choose conversation[0] when Rosie talks, conversation[1] when BeBe talks
        {
            if(npcDialogue.conversation.Length > 1 && npcDialogue.conversation[1] != null)
            currentConversation = npcDialogue.conversation[1];
        }
        //currentConversation = npcDialogue.conversation[1];
        dialogueActivated = true;
    }



    public void ObjectLockedDialogue(NPCDialogue npcDialogue)                               //This Method fetches the Dialogue, which is played when the Object is interacted with, but locked 
    {
        CurrentNPC = npcDialogue;
        //the array we are currently stepping through
        if (DMReference.CurrentCharacter.RosieActive == true && npcDialogue.conversation.Length > 2 && npcDialogue.conversation[2] != null)
        {
            currentConversation = npcDialogue.conversation[2];
        }
        else                                                                  //add a selector to choose conversation[2] when Rosie talks, conversation[3] when BeBe talks
        {
            if (npcDialogue.conversation.Length > 3 && npcDialogue.conversation[3] != null)
                currentConversation = npcDialogue.conversation[3];
        }
        //currentConversation = npcDialogue.conversation[1];
        dialogueActivated = true;
    }



    public void ForceDialogue(NPCDialogue npcDialogue)
    {
        CurrentNPC = npcDialogue;

        currentConversation = npcDialogue.conversation[0];

        dialogueActivated = true;
    }

    public void TurnOffDialogue(string Caller)
    {
        if(DMReference.MoveScript != null)
        {
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInput());            //Enable Inpput Again
            DMReference.MoveScript.StartCoroutine(DMReference.MoveScript.CallEnableInteract());         //Enable Interact Again
        }
        if (WriterIsRunning)
        {
            StopTypeWriter = true;
        }
        dialogueText.text = "";
        stepNum = 0;
        StopTypeWriter = false;
        dialogueActivated = false;
        currentConversation = null;
        typeWriterRoutine = null;
        if (optionsPanel != null) { optionsPanel.SetActive(false); }
        if (dialogueCanvas != null) { dialogueCanvas.SetActive(false); }
    }
}

public enum DialogueActors
{
    Rosie,
    Bebe,
    Silver,
    RosieShocked,
    Random,
    Branch,
};
