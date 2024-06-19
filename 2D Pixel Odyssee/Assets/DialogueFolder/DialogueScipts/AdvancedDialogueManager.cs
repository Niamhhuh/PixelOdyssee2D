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

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;

    //BUTTON REFERENCES
    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    //TYPEWRITER EFFECT
    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    SoundManagerHub SoundManagerHub;



    // Start is called before the first frame update
    void Start()
    {
        //FIND BUTTONS
        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionPanel");
        optionsPanel.SetActive(false);

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

        SoundManagerHub = GameObject.FindGameObjectWithTag("SoundManagerHub").GetComponent<SoundManagerHub>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActivated && Input.GetButtonDown("Interact") && canContinueText)
        {
            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();

            //Continue dialogue
            else
                PlayDialogue();
        }
    }

    void PlayDialogue()
    {
        Debug.Assert(currentConversation.actors.Length < stepNum, "stepNum out of range of actors. Did you forgot to assign it?");

        //If it's a random NPC
        if (currentConversation.actors[stepNum] == DialogueActors.Random)
            SetActorInfo(false);

        //If it's a recurring Character
        else
            SetActorInfo(true);

        //Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;                                                  //---------------------------------

        //If there is a branch...
        if (currentConversation.actors[stepNum] == DialogueActors.Branch)
        {
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
                optionButton[0].GetComponent<Button>().Select();
            }
        }

        //Keep routine from running multiple times at the same time
        if(typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        
        if(stepNum < currentConversation.dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypewriterEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        else
            optionsPanel.SetActive(true);
       
        dialogueCanvas.SetActive(true);
        stepNum += 1;

        SoundManagerHub.PlaySfxHub(SoundManagerHub.Dialog);
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

    private IEnumerator TypewriterEffect(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        bool addingRichTextTag = false;
        yield return new WaitForSeconds(.5f);
        foreach(char letter in line.ToCharArray())
        {
            if (Input.GetButtonDown("Interact"))
            {
                dialogueText.text = line;
                break;
            }

            //Check to see if we are working with rich text tags
            if(letter == '<' || addingRichTextTag)
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
        canContinueText = true;
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        //the array we are currently stepping through
        currentConversation = npcDialogue.conversation[0];
        
        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;

        dialogueActivated = false;
        optionsPanel.SetActive(false);
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
