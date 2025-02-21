using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImageButton : MonoBehaviour
{
    public Image CurrentImage;                      //Current Image used by the Button

    public Sprite RosieImage;                       //Rosie Seleted Image
    public Sprite BebeImage;                        //Bebe Selected Image

    public Sprite HighlightSwapButtonRosie;
    public Sprite HighlightSwapButtonBebe;

    AdvancedDialogueManager DialogueScript;
    PauseMenu PauseScript;
    DataManager DMReference;

    void Start()
    {
        CurrentImage = GetComponent<Image>();

        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager

        DialogueScript = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<AdvancedDialogueManager>();
        PauseScript = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();

        if (CurrentImage != null && DataManager.RosieActive == true && RosieImage != null)
        {
            CurrentImage.sprite = BebeImage;
        }

        if (CurrentImage != null && DataManager.RosieActive == false && BebeImage != null)
        {
            CurrentImage.sprite = RosieImage;
        }

    }

    // Method to swap the image on button click or Hover Exit(Clear Highlight)
    public void SwapImage()
    {
        if (DataManager.RosieActive == true && BebeImage != null && !PauseScript.InPause && !DialogueScript.InDialogue)
        {
            CurrentImage.sprite = RosieImage;
        }

        if (DataManager.RosieActive == false && BebeImage != null && !PauseScript.InPause && !DialogueScript.InDialogue)
        {
            CurrentImage.sprite = BebeImage;
        }

        //DMReference.FlickerSwitchChaButton();
    }

    //Call in Inspector on Pointer Enter
    public void HighlightImage()
    {
        if (CurrentImage != null && DataManager.RosieActive == true && RosieImage != null && !PauseScript.InPause && !DialogueScript.InDialogue)
        {
            CurrentImage.sprite = HighlightSwapButtonRosie;
        }

        if (CurrentImage != null && DataManager.RosieActive == false && RosieImage != null && !PauseScript.InPause && !DialogueScript.InDialogue)
        {
            CurrentImage.sprite = HighlightSwapButtonBebe;
        }
    }
}
