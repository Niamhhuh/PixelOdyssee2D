using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control player movement speed
    public float arrivalThreshold = 0.01f; // Adjust this to control how close the player needs to be to the target position to consider it arrived

    private bool isMoving = false;
    private Vector3 targetPosition;

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over a collider
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // If the mouse is over a collider
            if (hit.collider != null)
            {
                // Check if the collider belongs to a game object tagged as "Clickable"
                if (hit.collider.CompareTag("Clickable"))
                {
                    // Set target position to the position of the clicked object
                    targetPosition = hit.collider.gameObject.transform.position;

                    // Start moving towards the target position
                    isMoving = true;
                }
            }
        }

        // Move the player towards the target position
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the player has arrived at the target position
            if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold)
            {
                // Stop moving
                isMoving = false;
            }
        }
    }
}
