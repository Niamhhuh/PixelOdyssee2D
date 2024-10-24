using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiToMouse : MonoBehaviour
{
    private RectTransform rectTransform;
    public Transform player;
    public float playerSpeed = 5f;
    public Canvas canvas;
    public Vector3 targetPosition;
    private bool movePlayer = false;
    public bool AllowInput;
    public bool InventoryActive;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = player.position;
        AllowInput = true;
        InventoryActive = false;
    }

    public void DisableInput()
    {
        AllowInput = false;
    }

    public void EnableInput()
    {
        if(InventoryActive == false)
        {
            AllowInput = true;
        } 
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && AllowInput)
        {

            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

            worldPosition.y = -3.5f;
            worldPosition.z = 0f;

            rectTransform.position = worldPosition;

            targetPosition = new Vector3(worldPosition.x, player.position.y, player.position.z);

            movePlayer = true;

            GameObject.Find("MovePointer").GetComponent<Image>().enabled = true;
        }

        if (movePlayer)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerSpeed * Time.deltaTime);

            if (player.position == targetPosition)
            {
                movePlayer = false;
                GameObject.Find("MovePointer").GetComponent<Image>().enabled = false;
            }
        }

    }
}
