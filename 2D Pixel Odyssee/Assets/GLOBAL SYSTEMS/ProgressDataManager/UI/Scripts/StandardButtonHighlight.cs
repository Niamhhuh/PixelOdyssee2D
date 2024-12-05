using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StandardButtonHighlight : MonoBehaviour
{
    private Image CurrentImage;      //Current Image used by the Button

    private Sprite MainImage;        //Bebe Selected Image
    public Sprite SwapImage;        //Bebe Selected Image

    void Start()
    {
        CurrentImage = GetComponent<Image>();
        MainImage = CurrentImage.sprite;

    }

    // Method to swap the image on button click or Hover Exit(Clear Highlight)
    public void HighlightButton()
    {
        CurrentImage.sprite = SwapImage;
    }

    //Call in Inspector on Pointer Enter
    public void UnHighlightButton()
    {
        CurrentImage.sprite = MainImage;
    }
}
