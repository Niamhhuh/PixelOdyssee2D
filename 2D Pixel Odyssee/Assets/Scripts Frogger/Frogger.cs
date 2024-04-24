using System.Collections;
using UnityEngine;

public class Frogger : MonoBehaviour
{
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) {
            transform.rotation = Quaternion.Euler(0f,0f, 0f);
            Move(Vector3.up);
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        } 
        else if (Input.GetKeyDown(KeyCode.A)) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        } 
        else if (Input.GetKeyDown(KeyCode.D)) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {
        //transform.position += direction;
        Vector3 destination = transform.position + direction;
        
        StartCoroutine(Leap(destination));

    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 StartPosition = transform.position;
        
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(StartPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;  
    }
}
