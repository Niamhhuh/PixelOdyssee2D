using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToggleStartScreen : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
