using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageComponent;  // Das UI-Image
    public Sprite[] animationFrames; // Liste der Animationsframes
    public float frameRate = 0.1f; // Geschwindigkeit der Animation

    private int currentFrame;
    private float timer;
    private bool isHovered = false; // Steuert, ob die Animation läuft

    void Update()
    {
        if (isHovered && animationFrames.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= frameRate)
            {
                timer = 0;
                currentFrame = (currentFrame + 1) % animationFrames.Length;
                imageComponent.sprite = animationFrames[currentFrame];
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; // Startet die Animation beim Hovern
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // Stoppt die Animation, wenn die Maus das Element verlässt
        currentFrame = 0;  // Setzt das Bild zurück
        imageComponent.sprite = animationFrames[0];
    }
}
