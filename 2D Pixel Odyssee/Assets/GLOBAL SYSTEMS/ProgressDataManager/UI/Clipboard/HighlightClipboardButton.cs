using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightClipboardButton : MonoBehaviour
{
    public Image CurrentImage;      //Current Image used by the Button

    public GameObject TargetButton;
    public Sprite OriginalSprite;
    public Sprite HighlightSprite;

    AdvancedDialogueManager DialogueScript;
    PauseMenu PauseScript;

    DataManager DMReference;

    void Start()
    {
        if(TargetButton == null)
        {
            TargetButton = gameObject;
        }
        CurrentImage = TargetButton.GetComponent<Image>();

        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        DialogueScript = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<AdvancedDialogueManager>();
        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();

        if (CurrentImage != null)
        {
            CurrentImage.sprite = OriginalSprite;
        }
    }

    // Method to swap the image on button click or Hover Exit(Clear Highlight)
    public void ClearHighlight()
    {
        CurrentImage.sprite = OriginalSprite;
    }

    //Call in Inspector on Pointer Enter
    public void HighlightImage()
    {
        if(!DialogueScript.InDialogue && !PauseScript.InPause)
        {
            CurrentImage.sprite = HighlightSprite;
        }
    }
}
