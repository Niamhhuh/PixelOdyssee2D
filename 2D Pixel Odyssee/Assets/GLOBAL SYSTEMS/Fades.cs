using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//WHERE IS THE SCRIPT USED?
// --> PauseMenu, in functions: Start(), StartGameCoroutine(), GameQuit()
// --> ToggleStartScreen, in functions: Update()


//WHERE IS THIS SCRIPT PLACED?
// --> always where the PauseMenu Script is located --> GameManager in Startscreen and EventManager in Hubworld




namespace Fades                                     // wirte "using Fades;" atop every script to make usage of following functions easir (I think?)
{
    public class Class_Fades : MonoBehaviour
    {
        //-----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------Variables----------------------------------------------------------------------------
        [HideInInspector]
        public GameObject fadeObject;
        
        private Image fadeImage;
        private float fadeDuration = 1f;
        public static Class_Fades instance;

        //-----------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Public functions-------------------------------------------------------------------------

        private void Awake()
        {
            if (instance == null) {
                instance = this;
            }

            else {
                Debug.LogWarning("Fades already there - deleting other...");
                Destroy(gameObject);
            }
        }
        
        public void FindFadeObject() {                                  // First find the object where the blackscreen for the fade is located
            if (fadeObject == null) {
                fadeObject = GameObject.FindGameObjectWithTag("FadeInOut");
                if (fadeObject == null) {
                    Debug.LogError("Script Fade, Line 44: Fade object not found in the scene! Make sure it has the tag FadeInOut");
                    return;  // Stop execution if fadeObject can't be found
                }

                fadeImage = fadeObject.GetComponent<Image>();
                if (fadeImage == null) {
                    Debug.LogError("Script Fade, Line 50:Image component not found on Fade object! Ensure it has an Image component attached.");
                    return;  // Stop execution if no Image component is found
                }

                fadeImage.color = new Color(0, 0, 0, 1);
                fadeObject.SetActive(true); 
            }        
        }

        public IEnumerator StartFadeIn() {                                     // This is the function that starts the fade in coroutine
            FindFadeObject();
            if (fadeObject != null) {
                yield return StartCoroutine(FadeInCoroutine());
            }
        }

        public IEnumerator StartFadeOut() {                                    // this is the function that starts the fade out coroutine
            FindFadeObject();
            if (fadeObject != null) {
                yield return StartCoroutine(FadeOutCoroutine());
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------Enumerator Stuff------------------------------------------------------------------------
 
        private IEnumerator FadeOutCoroutine() {                        //fade out = black screen goes clear
            yield return new WaitForSeconds(0.5f);                      //for the fade out, we wait a bit in the beginning before it starts
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); 

                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null; 
            }

            fadeImage.color = new Color(0, 0, 0, 0);
            fadeObject.SetActive(false);
        }
        //-------------------------------------------

        private IEnumerator FadeInCoroutine() {                         //fade in = screen goes black
            fadeObject.SetActive(true);
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); 

                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null; 
            }

            fadeImage.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(1f);                        //for the fade in, we wait a bit in the end before it ends
        }
    }

}
