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

    //This Section Lists the variables used to adjust the panel size
    GameObject PanelPlate;
    int NameLegth;

    private void Start()
    {
        Displayed_Name = transform.GetChild(1).GetComponent<TMP_Text>();
        Display_Position = GetComponent<RectTransform>();
        MouseScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
        PanelPlate = GameObject.FindGameObjectWithTag("NamePanelPlate");

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


        //Adjust the Panel Scale based on the Characters
        NameLegth = Object_Name.Length;
        PanelPlate.transform.localScale = new Vector3(0.4f, PanelPlate.transform.localScale.y, PanelPlate.transform.localScale.z);

        for (int i = 0; i < NameLegth; i++) 
        {
            PanelPlate.transform.localScale = new Vector3(0.1f*i, PanelPlate.transform.localScale.y, PanelPlate.transform.localScale.z);
        }
        if(NameLegth <= 5)
        {
            PanelPlate.transform.localScale = new Vector3(PanelPlate.transform.localScale.x + 0.2f, PanelPlate.transform.localScale.y, PanelPlate.transform.localScale.z);
        }
        if (NameLegth > 10)
        {
            PanelPlate.transform.localScale = new Vector3(PanelPlate.transform.localScale.x - 0.2f, PanelPlate.transform.localScale.y, PanelPlate.transform.localScale.z);
        }
        if (NameLegth > 15)
        {
            PanelPlate.transform.localScale = new Vector3(PanelPlate.transform.localScale.x - 0.25f, PanelPlate.transform.localScale.y, PanelPlate.transform.localScale.z);
        }
    }


    public void DeactivateNameDisplay()                                 //Deactivate Object on Collider Exit
    {
        gameObject.SetActive(false);
    }


}
