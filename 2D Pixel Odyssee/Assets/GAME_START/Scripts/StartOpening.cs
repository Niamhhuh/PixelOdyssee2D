using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartOpening : MonoBehaviour
{
    public Sprite opening1;
    public Sprite opening2;

    private Image openings;
    private GameObject openingFull;

    static bool LogoPassed = false;

    //_______________________________________________________________________________
    //_______Basic functions below___________________________________________________

    void Start() {
        openings = GameObject.Find("Opening").GetComponent<Image>();
        openingFull = GameObject.Find("Canvas_Opening");

        openings.sprite = opening1;                          //make sure everything starts well...
        openingFull.SetActive(false);
        if (!LogoPassed)
        {
            openingFull.SetActive(true);
            openings.gameObject.SetActive(true);

            StartCoroutine(updateLoading());
        }
    }

    //_______________________________________________________________________________
    //_______Enumerator below________________________________________________________

    private IEnumerator updateLoading() {                           //same as in Eliza opening script
        yield return new WaitForSeconds(4f);

        CanvasGroup cg = openings.GetComponent<CanvasGroup>();      //get the canvas group for alpha
        if (cg == null) {                                           //if it doesn't exits, create one
            cg = openings.gameObject.AddComponent<CanvasGroup>();
        } 

        float alpha = 1f;                                           //fade the image out
        while (alpha > 0f) {
            alpha -= Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        openings.sprite = opening2;                                 //change sprite
        alpha = 0f;                                             
        while (alpha < 1f) {                                        //fade the new image in
            alpha += Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        alpha = 1f;
        while (alpha > 0f) {                                        //fade everything out
            alpha -= Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }

        cg = openingFull.gameObject.GetComponent<CanvasGroup>();    //change the canvas group from Backdrop to Canvas_Opening
        if (cg == null) {                                           //same procedure as above
            cg = openingFull.gameObject.AddComponent<CanvasGroup>();
        } 
        alpha = 1f;
        while (alpha > 0f) {                                        //fade everything out
            alpha -= Time.deltaTime;
            cg.alpha = alpha;
            yield return null;
        }
        LogoPassed = true;
        yield return new WaitForSeconds(1f);
        openingFull.SetActive(false);                               //set it all inactive
    }
}
