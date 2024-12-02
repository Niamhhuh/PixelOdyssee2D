using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Street : MonoBehaviour
{
	public AudioSource theMusic;

	public bool startPlaying;

	public BeatScroller theBS;

	public static GameManager_Street instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying){
        	if(Input.anyKeyDown){
        		startPlaying = true;
        		theBS.hasStarted = true;

        		theMusic.Play();
        	}
        }
    }

    public void NoteHit(){
    	Debug.Log("NoteHit");
    }

    public void NoteMissed(){
    	Debug.Log("NoteMissed");
    }
}
