using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Street : MonoBehaviour
{
	public AudioSource theMusic;

	public bool startPlaying;

	public BeatScroller theBS;

	public static GameManager_Street instance;

    public Animator rosieAnimator;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GameObject Rosie = GameObject.Find("Rosie");
        rosieAnimator = Rosie.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying){
        	if(Input.anyKeyDown){
        		startPlaying = true;
        		theBS.hasStarted = true;

        		theMusic.Play();
                rosieAnimator.Play("Rosie_Idle_Street");
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
