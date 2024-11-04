using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference TitelScreen;
    // Start is called before the first frame update
    void Start()
    {
        playTitelScreen();
    }

    public void playTitelScreen()
    {
        RuntimeManager.PlayOneShot(TitelScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
