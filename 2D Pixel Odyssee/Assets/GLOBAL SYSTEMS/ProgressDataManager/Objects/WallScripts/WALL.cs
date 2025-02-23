using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WALL : MonoBehaviour
{
    DataManager DMReference;
    public GameObject WallTrigger;

    private UiToMouse PointerScript;

    public bool WallClicked;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        WallTrigger.SetActive(false);
        PointerScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();
    }


    private void OnMouseOver()
    {
        WallTrigger.SetActive(true);

        if (Input.GetMouseButtonUp(0))
        {
            PointerScript.WallScript = this;
            WallClicked = true;
        }
    }


    private void OnMouseExit()
    {
        if(!WallClicked)
        {
            WallTrigger.SetActive(false);
        }
        
        DMReference.MoveScript.EnableInput();
        DMReference.MoveScript.EnableInteract();
    }
        
}
