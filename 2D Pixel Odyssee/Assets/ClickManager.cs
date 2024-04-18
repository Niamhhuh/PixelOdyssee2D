using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
   public Transform player;
   GameManager gameManager;
   
   private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

   public void GoToItem(ItemData item)
    {
        StartCoroutine(gameManager.MoveToPoint(player,item.goToPoint.position));
        TryGettingItem(item);
    }

   

    private void TryGettingItem(ItemData item)
    {
        if (item.requiredItemID == -1 || GameManager.collectedItems.Contains(item.requiredItemID))
        {
            GameManager.collectedItems.Add(item.itemID);
            Debug.Log("Item Collected");
        }
    }
}
