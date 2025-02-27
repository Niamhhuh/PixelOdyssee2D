using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fades;

public class ToggleStartScreen : MonoBehaviour
{
    public PauseMenu script_pause;
    public bool steuerungOff = false;

    public void Start() {
        StartCoroutine(Class_Fades.instance.StartFadeOut());        //makes sure the fade in happens because below, we turn off the pause script
        script_pause.enabled = false;                               //turn off pause script so it does not overlap with the beginning steuerung
    }
    public void Update ()                      
    {
        if (Class_Fades.instance.fadeObject.activeInHierarchy == false && gameObject.activeInHierarchy == true) {       //first condition checks if there is not fade active
            Time.timeScale = 0f;
            steuerungOff = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            script_pause.enabled = true;
            steuerungOff = true;
            gameObject.SetActive(false);
        }
    }
}
