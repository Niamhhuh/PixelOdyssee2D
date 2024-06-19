using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel

    SoundManagerHub SoundManagerHub;

    void Start()
    {
        // Ensure the inventory panel is hidden at the start
        inventoryPanel.SetActive(false);

        SoundManagerHub = GameObject.FindGameObjectWithTag("SoundManagerHub").GetComponent<SoundManagerHub>();
    }

    void Update()
    {
        // Check if the player presses the "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the active state of the inventory panel
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            SoundManagerHub.PlaySfxHub(SoundManagerHub.OpenInventar);
        }
    }
}
