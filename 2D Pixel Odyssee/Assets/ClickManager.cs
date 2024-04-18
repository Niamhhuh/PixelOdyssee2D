using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
   float moveSpeed = 3.5f, moveAccuracy = 0.15f;
   public Transform player;

   public void GoToItem(ItemData item)
    {
        StartCoroutine(MoveToPoint(item.goToPoint.position));
        TryGettingItem(item);
    }

    public IEnumerator MoveToPoint(Vector2 point)
    { 
        Vector2 positionDifference = point - (Vector2)player.position; // calculate position difference
        while (positionDifference.magnitude > moveAccuracy)           // stop when we are near the point
        {
            player.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime); // move in direction frame after frame
            positionDifference = point - (Vector2)player.position;                       // recalculate position difference
            yield return null; 
        }
        player.position = point;

        yield return null;
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
