using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerUpHandler
{
    PauseMenu PauseController;

    // Start is called before the first frame update
    void Start()
    {
        PauseController = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseMenu>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>().InCatScene)
        {
            PauseController.CallPause();
        }
    }

}
