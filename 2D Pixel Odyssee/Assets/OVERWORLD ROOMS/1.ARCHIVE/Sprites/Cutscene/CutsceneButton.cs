using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneButton : MonoBehaviour
{
    public GameObject CallPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivatePanel()
    {
        CallPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
