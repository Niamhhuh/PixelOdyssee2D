using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fades;

public class GloveScript : MonoBehaviour
{
    public static bool CallGlove;
    public static int GloveProgress;

    private GameObject GloveOfPower;
    private static GameObject [] Gloves = new GameObject[5];

    bool AnimationFinished;

    public Image imageToFade;           // The UI Image you want to fade in
    public float fadeDuration = 10f;     // The time it takes to fade in

    public UiToMouse MoveScript;

    void Start()
    {
        AnimationFinished = false;
        GloveOfPower = this.gameObject;

        Gloves[0] = GameObject.FindGameObjectWithTag("Glove_0");
        Gloves[1] = GameObject.FindGameObjectWithTag("Glove_1");
        Gloves[2] = GameObject.FindGameObjectWithTag("Glove_2");
        Gloves[3] = GameObject.FindGameObjectWithTag("Glove_3");
        Gloves[4] = GameObject.FindGameObjectWithTag("Glove_4");

        MoveScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();

        foreach (GameObject Glove in Gloves)
        {
            Glove.SetActive(false);
        }

        GloveOfPower.SetActive(false);
    }

    public void ActivateGlove()
    {
        GloveOfPower.SetActive(true);

        if(GloveProgress - 1 >= 0)
        {
            Gloves[GloveProgress - 1].SetActive(true);
            StartCoroutine(FadeInImage(GloveProgress - 1));
        }
        if(GloveProgress - 1 < 0)
        {
            StartCoroutine(FadeInImage2(GloveProgress));
        }
    }


    public void CloseGlove()
    {
        if(AnimationFinished)
        {
            MoveScript.DisableInput();
            MoveScript.DisableInteract();
            StartCoroutine(FadeOut(GloveProgress));
        }
    }


    private IEnumerator FadeInImage(int Glove)
    {
        imageToFade = Gloves[Glove].GetComponent<Image>();
        float timeElapsed = 0f;
        Color imageColor = imageToFade.color;

        // Set the initial alpha to 0 (fully transparent)
        imageColor.a = 0f;
        imageToFade.color = imageColor;

        // Gradually increase the alpha from 0 to 1 (fully visible)
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            imageColor.a = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            imageToFade.color = imageColor;

            yield return null;  // Wait for the next frame
        }

        // Ensure the image is fully opaque after the fade is complete
        imageColor.a = 1f;
        imageToFade.color = imageColor;
        StartCoroutine(FadeInImage2(GloveProgress));
    }

    private IEnumerator FadeInImage2(int Glove)
    {
        Gloves[GloveProgress].SetActive(true);
        imageToFade = Gloves[Glove].GetComponent<Image>();
        float timeElapsed = 0f;
        Color imageColor = imageToFade.color;

        // Set the initial alpha to 0 (fully transparent)
        imageColor.a = 0f;
        imageToFade.color = imageColor;

        // Gradually increase the alpha from 0 to 1 (fully visible)
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            imageColor.a = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            imageToFade.color = imageColor;

            yield return null;  // Wait for the next frame
        }

        // Ensure the image is fully opaque after the fade is complete
        imageColor.a = 1f;
        imageToFade.color = imageColor;
        AnimationFinished = true;
        if(GloveProgress - 1 >= 0)
        {
            Gloves[GloveProgress - 1].SetActive(false);
        }
    }


    private IEnumerator FadeOut(int Glove)
    {
        imageToFade = Gloves[Glove].GetComponent<Image>();
        float timeElapsed = 0f;
        Color imageColor = imageToFade.color;

        // Set the initial alpha to 0 (fully transparent)
        imageColor.a = 1f;
        imageToFade.color = imageColor;

        // Gradually increase the alpha from 0 to 1 (fully visible)
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            imageColor.a = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            imageToFade.color = imageColor;

            yield return null;  // Wait for the next frame
        }

        // Ensure the image is fully opaque after the fade is complete
        imageColor.a = 0f;
        imageToFade.color = imageColor;
        //Gloves[GloveProgress].SetActive(false);

        MoveScript.Activate_CallEnableInteract();
        MoveScript.Activate_CallEnableInput();
        MoveScript.targetPosition = MoveScript.player.position;
        GloveOfPower.SetActive(false);
        CallGlove = false;
        //GloveProgress = 0;
    }
}