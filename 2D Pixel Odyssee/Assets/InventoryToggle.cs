using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel

    void Start()
    {
        // Ensure the inventory panel is hidden at the start
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Check if the player presses the "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the active state of the inventory panel
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
