using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab for the arrow object
    public List<Sprite> arrowSprites; // Sprites for different arrow types
    public List<KeyCode> arrowKeys; // Matching keys for the arrow types

    public float spawnInterval = 1f; // Time between spawns
    public float spawnYMin = -4f; // Minimum Y position
    public float spawnYMax = 4f; // Maximum Y position
    public float spawnXOffset = 10f; // Spawn position offset off-screen
    public BeatScroller theBS;
    public bool startSpawn;
    public Transform arrowSpawner;
    public bool isSpawning = false; // Toggle spawning on/off

    //private bool isCoroutineRunning = false;

    public int currentWave = 0;
    public int totalWaves = 3;
    private int arrowsPerWave = 9;
    private int arrowsSpawnedInWave = 0;

    public Text waveText;
    public GameObject wavePopup;

    
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

    	while(currentWave < totalWaves){
    		currentWave++;
    		arrowsSpawnedInWave = 0;
    		yield return StartCoroutine(ShowWavePopup());

    		while (arrowsSpawnedInWave < arrowsPerWave){
    			SpawnArrow();
    			arrowsSpawnedInWave++;
    			yield return new WaitForSeconds(spawnInterval);
    		}
    		yield return new WaitForSeconds(7f);
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
            noteObject.arrowSprites = arrowSprites;
            noteObject.arrowKeys = arrowKeys;
        }
    }

    private IEnumerator ShowWavePopup(){
    	wavePopup.SetActive(true);
    	waveText.text = "Wave " + currentWave.ToString();
    	yield return new WaitForSeconds(2f);
    	wavePopup.SetActive(false);
    }
}
