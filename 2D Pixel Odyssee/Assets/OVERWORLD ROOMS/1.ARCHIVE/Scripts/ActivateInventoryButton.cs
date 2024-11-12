using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInventoryButton : MonoBehaviour
{
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject.FindObjectOfType<TutorialToggleButtons>().GetComponent<TutorialToggleButtons>().ActivateInventoryButton();
        }
    }
}
