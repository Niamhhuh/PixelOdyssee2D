using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab for the arrow object
    public List<Sprite> arrowSprites; // Sprites for different arrow types
    public List<KeyCode> arrowKeys; // Matching keys for the arrow types

    public float spawnInterval = 0.5f; // Time between spawns //previously 1 with 120BPM
    public float spawnYMin = -4f; // Minimum Y position
    public float spawnYMax = 4f; // Maximum Y position
    public float spawnXOffset = 10f; // Spawn position offset off-screen
    public BeatScroller theBS;
    public NoteObject theNO;
    public GameManager_Street theGM;
    public bool startSpawn;
    public Transform arrowSpawner;
    public bool isSpawning = false; // Toggle spawning on/off

    //private bool isCoroutineRunning = false;

    public int currentWave = 0;
    public int totalWaves = 3;
    private int arrowsPerWave = 9;
    private int arrowsSpawnedInWave = 0;
    public int round = 0;

    public TextMeshProUGUI waveText;
    public GameObject wavePopup;
    public GameManager_Street GMref;
    public TextMeshProUGUI roundText;
    public GameObject roundPopop;
    public GameObject sourceObject;
    public TriggerAnimation animator;
    //public GameObject SilverHp1;
    //public GameObject SilverHp2;

    public GameObject Sprechblase;
    public bool StopRound2Trigger = false;

    private EventInstance SFCountdown; //Sound


    void Awake(){
        animator = GameObject.Find("Round1").GetComponent<TriggerAnimation>();
    }

    public void Start()
    {
        SFCountdown = AudioManager_Startscreen.instance.CreateEventInstance(Fmod_Events.instance.SFCountdown); //Sound
    }
    void Update()
    {
    	if (theBS.hasStarted && !isSpawning){

    		StartCoroutine(SpawnWaves());
    	}
        
    }

    /*private IEnumerator SpawnArrows()
    {
    	isCoroutineRunning = true;
        while (isSpawning)
        {
            SpawnArrow();
            yield return new WaitForSeconds(spawnInterval);	
        }
        isCoroutineRunning = false;
    }*/

    private IEnumerator SpawnWaves(){
    	isSpawning = true;
        yield return new WaitForSeconds(3f);
    	while(currentWave < totalWaves){
    		currentWave++;
    		arrowsSpawnedInWave = 0;
    		//yield return StartCoroutine(ShowWavePopup());

    		while (arrowsSpawnedInWave < arrowsPerWave){
    			SpawnArrow();
    			arrowsSpawnedInWave++;
    			yield return new WaitForSeconds(0.5f);  //previously ...(spawnInterval);
    		}
    		if(currentWave == totalWaves && arrowsSpawnedInWave == 9){

    			GMref.EndofGame();
    		}
    		yield return new WaitForSeconds(5f);   //previously 9 with 120BPM
    		
    		if (currentWave == totalWaves && theNO.round2){
                if (!StopRound2Trigger){
                    yield return StartCoroutine(ShowRoundPopup());
                    StopRound2Trigger = true;
                }
                //yield return new WaitForSeconds(3f);
    			currentWave = 0;
    			theGM.allowInput = true;	
    		}
    		
    		
    	}
    	isSpawning = false;
    }

    private void SpawnArrow()
    {
        // Randomize the arrow type
        int randomIndex = Random.Range(0, arrowSprites.Count);

        // Instantiate arrow off-screen to the left
        Vector3 spawnPosition = new Vector3(-spawnXOffset, arrowSpawner.position.y, 0f);
        GameObject newArrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity, arrowSpawner);
        newArrow.SetActive(true);
        // Assign the sprite and key to the new arrow
        NoteObject noteObject = newArrow.GetComponent<NoteObject>();
        SpriteRenderer spriteRenderer = newArrow.GetComponent<SpriteRenderer>();

        if (noteObject != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = arrowSprites[randomIndex];
            noteObject.keyToPress = arrowKeys[randomIndex];   

            ListProvider listProvider = sourceObject.GetComponent<ListProvider>();
            if (listProvider != null)
        {
            noteObject.arrowSprites = new List<Sprite>(listProvider.newArrowSprites);
            noteObject.arrowKeys = new List<KeyCode>(listProvider.newArrowKeys);
        }
        }
        
    }

    private IEnumerator ShowWavePopup(){
    	wavePopup.SetActive(true);
    	waveText.text = "Wave " + currentWave.ToString();
    	yield return new WaitForSeconds(2f);
    	wavePopup.SetActive(false);
    }

    private IEnumerator ShowRoundPopup(){

        Sprechblase.SetActive(true);
        yield return new WaitForSeconds(4f);
        Sprechblase.SetActive(false);
    	animator.PlayScaleAnimationRound2();

        SFCountdown.start(); //Sound

    	yield return new WaitForSeconds(2f);
    }
}
