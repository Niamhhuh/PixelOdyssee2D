using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImageButton : MonoBehaviour
{
    public Image CurrentImage;      //Current Image used by the Button

    public Sprite RosieImage;       //Rosie Seleted Image
    public Sprite BebeImage;        //Bebe Selected Image

    public Sprite HighlightSwapButtonRosie;
    public Sprite HighlightSwapButtonBebe;

    DataManager DMReference;

    void Start()
    {
        CurrentImage = GetComponent<Image>();

        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();          //Find and Connect to DataManager
        if (CurrentImage != null && DMReference.CurrentCharacter.RosieActive == true && RosieImage != null)
        {
            CurrentImage.sprite = BebeImage;
        }

        if (CurrentImage != null && DMReference.CurrentCharacter.RosieActive == false && BebeImage != null)
        {
            CurrentImage.sprite = RosieImage;
        }

    }

    // Method to swap the image on button click or Hover Exit(Clear Highlight)
    public void SwapImage()
    {
        if (DMReference.CurrentCharacter.RosieActive == true && BebeImage != null)
        {
            CurrentImage.sprite = RosieImage;
        }

        if (DMReference.CurrentCharacter.RosieActive == false && BebeImage != null)
        {
            CurrentImage.sprite = BebeImage;
        }

        //DMReference.FlickerSwitchChaButton();
    }

    //Call in Inspector on Pointer Enter
    public void HighlightImage()
    {
        if (CurrentImage != null && DMReference.CurrentCharacter.RosieActive == true && RosieImage != null)
        {
            CurrentImage.sprite = HighlightSwapButtonRosie;
        }

        if (CurrentImage != null && DMReference.CurrentCharacter.RosieActive == false && RosieImage != null)
        {
            CurrentImage.sprite = HighlightSwapButtonBebe;
        }
    }
}