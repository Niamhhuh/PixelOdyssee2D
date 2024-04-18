using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new List<int>();
    static float moveSpeed = 3.5f, moveAccuracy = 0.15f;

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Vector2 positionDifference = point - (Vector2)myObject.position; // calculate position difference
        while (positionDifference.magnitude > moveAccuracy)           // stop when we are near the point
        {
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime); // move in direction frame after frame
            positionDifference = point - (Vector2)myObject.position;                       // recalculate position difference
            yield return null;
        }
        myObject.position = point;

        yield return null;
    }
}
