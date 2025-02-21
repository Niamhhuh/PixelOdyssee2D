using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;
    public GameObject uiPanel; // Dein UI-Panel für die Einleitung

    private bool wasActivated = false;
    private bool fadeStarted = false;

    private void Start()
    {
        fadeCanvasGroup.alpha = 1; // Schwarzer Screen bleibt sichtbar
        Debug.Log("Skript gestartet!"); // Debug
        StartCoroutine(CheckUIPanelState()); // Startet die Überwachungs-Coroutine
    }

    private IEnumerator CheckUIPanelState()
    {
        while (!fadeStarted)
        {
            if (uiPanel == null)
            {
                Debug.LogError("Fehler: UI-Panel nicht zugewiesen!");
                yield break;
            }

            // Debug: Überprüfe, ob das Panel wirklich aktiv in der Hierarchie ist
            Debug.Log("Panel aktiv (activeInHierarchy): " + uiPanel.activeInHierarchy);

            // Warten Sie hier einen Moment, um den Zustand korrekt zu erfassen
            yield return new WaitForSeconds(0.1f);

            // Überprüfe, ob das Panel aktiviert wurde
            if (uiPanel.activeInHierarchy && !wasActivated)
            {
                wasActivated = true;
                Debug.Log("UI-Panel wurde aktiviert!"); // Debug
            }

            // Wenn das Panel aktiviert wurde und jetzt deaktiviert wird
            if (wasActivated && !uiPanel.activeInHierarchy && !fadeStarted)
            {
                fadeStarted = true;
                Debug.Log("UI-Panel wurde deaktiviert! Starte Fade-In..."); // Debug
                StartCoroutine(FadeIn());
            }

            yield return null; // Warten auf den nächsten Frame
        }
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("Fade-In gestartet!"); // Debug
        float elapsedTime = 0;
        float startAlpha = 1f;
        float targetAlpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            t = EaseInOutQuad(t); // Easy Ease für smoothes Fading
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
        Debug.Log("Fade-In abgeschlossen!"); // Debug
    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    }
}
