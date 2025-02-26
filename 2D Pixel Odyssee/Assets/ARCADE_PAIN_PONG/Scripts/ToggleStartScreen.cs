using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fades;

public class ToggleStartScreen : MonoBehaviour
{
    public bool steuerungOff = false;
    public void Update ()                      
    {
        if (Class_Fades.instance.fadeObject.activeInHierarchy == false && gameObject.activeInHierarchy == true) {       //first condition checks if there is not fade active
            Time.timeScale = 0f;
            steuerungOff = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            steuerungOff = true;
            gameObject.SetActive(false);
        }
    }
}
