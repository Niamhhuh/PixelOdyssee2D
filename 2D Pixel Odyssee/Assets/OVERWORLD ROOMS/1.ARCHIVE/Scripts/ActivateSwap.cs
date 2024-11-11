using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwap : MonoBehaviour
{
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject.FindObjectOfType<TutorialToggleButtons>().GetComponent<TutorialToggleButtons>().ActivateSwitchButton();
        }
    }
}
