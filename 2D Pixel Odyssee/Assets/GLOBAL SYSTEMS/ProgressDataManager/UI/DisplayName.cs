using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayName : MonoBehaviour
{
    //Image NameBox;                         //Background for the displayed Object_Name
    TMP_Text Displayed_Name;                //Container for the Object_Name
    RectTransform Display_Position;
    UiToMouse MouseScript;

    private void Start()
    {
        Displayed_Name = transform.GetChild(0).GetComponent<TMP_Text>();
        Display_Position = GetComponent<RectTransform>();
        MouseScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();

        gameObject.SetActive(false);
    }

    public void SetDisplayPosition()                                    //Called in Object while Mouse hovers over Object - Set Display Position
    {
        Display_Position.position = MouseScript.PermanentmousePosition;
    }

    public void ActivateNameDisplay (string Object_Name)                //Activate Display on Mouse Enter, pass Object Name
    {
        gameObject.SetActive(true);
        Displayed_Name.text = Object_Name;
    }


    public void DeactivateNameDisplay()                                 //Deactivate Object on Collider Exit
    {
        gameObject.SetActive(false);
    }


}
