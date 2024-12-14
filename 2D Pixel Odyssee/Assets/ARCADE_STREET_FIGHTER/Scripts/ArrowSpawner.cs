using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isSpawning = true; // Toggle spawning on/off

    private bool isCoroutineRunning = false;
    
    void Update()
    {
    	if (theBS.hasStarted && !isCoroutineRunning){

    		StartCoroutine(SpawnArrows());
    	}
        
    }

    private IEnumerator SpawnArrows()
    {
    	isCoroutineRunning = true;
        while (isSpawning)
        {
            SpawnArrow();
            yield return new WaitForSeconds(spawnInterval);	
        }
        isCoroutineRunning = false;
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
}
