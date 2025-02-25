using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fades;

public class CreditsScrollWithImages : MonoBehaviour
{
    private float startSpeed = 50f;                                     // Initial scrolling speed
    private float endSpeed = 5f;                                        // Slowest speed at the end
    private float slowdownStartY = 1750;                                // When the slowdown begins
    private float slowdownDistance = 300f;                              // Over how much distance it slows
    private float stopY = 2039f;                                        // Final Y position where it stops

    private RectTransform creditsRect;                                  // This is for the credits

    //_______________________________________________________________________________
    //_______Stuff for the polaroid images___________________________________________

    [System.Serializable]
    public class ImageTrigger {
        public Image image;                                             // The image to show
        public float triggerY;                                          // Y position at which it appears
    }

    public List<ImageTrigger> imageTriggers = new List<ImageTrigger>(); // List of images & positions
    private HashSet<Image> activatedImages = new HashSet<Image>();      // Track shown images

    //_______________________________________________________________________________
    //_______Basic functions below___________________________________________________

    void Start() {
        creditsRect = GetComponent<RectTransform>();

        foreach (var trigger in imageTriggers) {
            trigger.image.gameObject.SetActive(false);                  // Hide all images at the start
        }

        StartCoroutine(ScrollCredits());
    }

    //_______________________________________________________________________________
    //_______Enumerator below________________________________________________________

    IEnumerator ScrollCredits() {
        float currentSpeed = startSpeed;

        while (creditsRect.anchoredPosition.y < stopY) {
            float yPos = creditsRect.anchoredPosition.y;
            
            if (yPos >= slowdownStartY) {                               // Start slowing down when reaching slowdownStartY
                float t = Mathf.Clamp01((yPos - slowdownStartY) / slowdownDistance);
                currentSpeed = Mathf.Lerp(startSpeed, endSpeed, t);
            }
            
            creditsRect.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime; // Move the text upwards
            
            foreach (var trigger in imageTriggers) {                    // Show images at the correct positions
                if (yPos >= trigger.triggerY && !activatedImages.Contains(trigger.image)) {
                    StartCoroutine(FadeIn(trigger.image));
                    activatedImages.Add(trigger.image);
                }
            }

            yield return null;                                          // Wait for the next frame
        }

        creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, stopY);  // Stop the text exactly at stopY
    }

    //----------------------------------------------------------------

    IEnumerator FadeIn(Image img) {
        yield return new WaitForSeconds(2f);       
        img.gameObject.SetActive(true);
        CanvasGroup cg = img.GetComponent<CanvasGroup>();
        if (cg == null) cg = img.gameObject.AddComponent<CanvasGroup>();

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
    }
}