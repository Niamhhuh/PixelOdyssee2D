using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


                                                                    //WHERE CAN YOU FIND THIS SCRIPT?
                                                                    //--> on the object Canvas_Start in z_Eliza and in Folder BEBE_DREAM


public class Eliza_start : MonoBehaviour
{
    public Sprite bebe_sleep;
    public Sprite bebe_awake;
    public TextMeshProUGUI textLoading;

    private Image loadingSprite;
    private GameObject loadingCanvas;


    //_______________________________________________________________________________
    //_______Basic functions below___________________________________________________

    void Start() {
        loadingSprite = GameObject.Find("LoadingSprite").GetComponent<Image>();
        loadingCanvas = GameObject.Find("Canvas_Start");

        loadingSprite.sprite = bebe_sleep;                          //make sure everything starts well...
        textLoading.text = "System wird  hochgefahren...";

        loadingCanvas.SetActive(true);
        loadingSprite.gameObject.SetActive(true);
        textLoading.gameObject.SetActive(true);

        StartCoroutine(updateLoading());
    }


    //_______________________________________________________________________________
    //_______Enumerator below________________________________________________________

    private IEnumerator updateLoading() {                           //same as in credits script, just a few more repeats
        yield return new WaitForSeconds(5f);

        CanvasGroup cg = loadingSprite.GetComponent<CanvasGroup>(); //get the canvas group for alpha
        if (cg == null) {                                           //if it doesn't exits, create one
            cg = loadingSprite.gameObject.AddComponent<CanvasGroup>();
        } 

        float alpha = 1f;                                           //fade the image out
        while (alpha > 0f) {
            alpha -= Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        loadingSprite.sprite = bebe_awake;                          //change sprite
        textLoading.text = "Klicken, um das System zu steuern.";    //change text
        alpha = 0f;                                             
        while (alpha < 1f) {                                        //fade the new image in
            alpha += Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(4f);

        cg = loadingCanvas.gameObject.GetComponent<CanvasGroup>();  //change the canvas group from Backdrop to Canvas_Start
        if (cg == null) {                                           //same procedure as above
            cg = loadingCanvas.gameObject.AddComponent<CanvasGroup>();
        } 
        alpha = 1f;
        while (alpha > 0f) {                                        //fade everything out
            alpha -= Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        loadingCanvas.SetActive(false);                             //set it all inactive
    }
}
