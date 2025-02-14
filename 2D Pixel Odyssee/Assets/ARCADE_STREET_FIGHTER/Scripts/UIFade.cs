using System.Collections;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;



    IEnumerator FadeInOut()
    {
        yield return Fade(0, 1); // Fade In
        yield return new WaitForSeconds(1f);
        yield return Fade(1, 0); // Fade Out
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}
