using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fades;

public class ToggleStartScreen : MonoBehaviour
{
    public void Update ()                      
    {
        if (Class_Fades.instance.fadeObject.activeInHierarchy == false && gameObject.activeInHierarchy == true) {       //first condition checks if there is not fade active
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
