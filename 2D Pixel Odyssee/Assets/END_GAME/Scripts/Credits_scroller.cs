using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fades;

public class CreditsScrollWithImages : MonoBehaviour
{
    private float startSpeed = 50f;  // Initial scrolling speed
    private float endSpeed = 5f;     // Slowest speed at the end
    private float slowdownStartY = 1750; // When the slowdown begins
    private float slowdownDistance = 300f; // Over how much distance it slows
    private float stopY = 2039f;     // Final Y position where it stops

    private RectTransform textRect;  // Assign the credits text RectTransform

    [System.Serializable]
    public class ImageTrigger
    {
        public Image image;    // The image to show
        public float triggerY; // Y position at which it appears
    }

    public List<ImageTrigger> imageTriggers = new List<ImageTrigger>(); // List of images & positions
    private HashSet<Image> activatedImages = new HashSet<Image>();  // Track shown images

    void Start()
    {
        textRect = GetComponent<RectTransform>();
        // Hide all images at the start
        foreach (var trigger in imageTriggers)
        {
            trigger.image.gameObject.SetActive(false);
        }

        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        float currentSpeed = startSpeed;

        while (textRect.anchoredPosition.y < stopY)
        {
            float yPos = textRect.anchoredPosition.y;

            // Start slowing down when reaching slowdownStartY
            if (yPos >= slowdownStartY)
            {
                float t = Mathf.Clamp01((yPos - slowdownStartY) / slowdownDistance);
                currentSpeed = Mathf.Lerp(startSpeed, endSpeed, t);
            }

            // Move the text upwards
            textRect.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;

            // Show images at the correct positions
            foreach (var trigger in imageTriggers)
            {
                if (yPos >= trigger.triggerY && !activatedImages.Contains(trigger.image))
                {
                    StartCoroutine(FadeIn(trigger.image));
                    activatedImages.Add(trigger.image);
                }
            }

            yield return null; // Wait for the next frame
        }

        // Stop the text exactly at stopY
        textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, stopY);
    }

    IEnumerator FadeIn(Image img)
    {
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