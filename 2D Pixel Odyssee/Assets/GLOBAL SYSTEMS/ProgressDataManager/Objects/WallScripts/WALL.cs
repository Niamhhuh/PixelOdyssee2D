using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WALL : MonoBehaviour
{
    DataManager DMReference;
    public GameObject WallTrigger;

    // Start is called before the first frame update
    void Start()
    {
        DMReference = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        WallTrigger.SetActive(false);
    }


    private void OnMouseOver()
    {
        WallTrigger.SetActive(true);
    }

    private void OnMouseExit()
    {
        WallTrigger.SetActive(false);
        DMReference.MoveScript.EnableInput();
        DMReference.MoveScript.EnableInteract();
    }
        
}
