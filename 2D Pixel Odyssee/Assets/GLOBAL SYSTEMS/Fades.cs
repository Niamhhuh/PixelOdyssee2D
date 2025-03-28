using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//WHERE IS THE SCRIPT USED?
// --> PauseMenu, in functions: Start(), StartGameCoroutine(), GameQuit()
// --> ToggleStartScreen, in functions: Start(), Update()


//WHERE IS THIS SCRIPT PLACED?
// --> always where the PauseMenu Script is located --> GameManager in Startscreen and EventManager in Hubworld


namespace Fades                                     // write "using Fades;" atop every script to make usage of following functions easir (I think?)
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

        private UiToMouse MoveScript;                                                                      //CONNECT MOVE SCRIPT
        private GloveScript GloveConnect;

        private UiToMouse script_uitomouse;     	                                                        //reference to script UImouse so we can turn off the footsteps accordingly

        //-----------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Basic functions-------------------------------------------------------------------------
        private void Awake() {
            if (instance == null) {
                instance = this;
            }

            else {
                Debug.LogWarning("Fades already there - deleting other...");
                Destroy(gameObject);
            }

            //-------------------------------------------------------Felix' stuff
            if (GameObject.FindGameObjectWithTag("Pointer") != null) {
                MoveScript = GameObject.FindGameObjectWithTag("Pointer").GetComponent<UiToMouse>();             //CONNECT MOVE SCRIPT
            }
            
            if(GameObject.FindGameObjectWithTag("GloveOfPower")!= null) {
                GloveConnect = GameObject.FindGameObjectWithTag("GloveOfPower").GetComponent<GloveScript>();
            }

            //---------------------find script UiMouse in case it is in the scene
            script_uitomouse = FindObjectOfType<UiToMouse>();
            if (script_uitomouse == null) {
                Debug.Log("no Pointer script in this scene");
                return;
            }
            else {
                Debug.Log("Footsteps stopped");
                script_uitomouse.stopSound();
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Public functions-------------------------------------------------------------------------
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

        public IEnumerator StartFadeIn() {                                     // This is the function that starts the fade in coroutine (everytime when we are in a new scene)
            FindFadeObject();
            
            if (fadeObject != null) {
                yield return StartCoroutine(FadeInCoroutine());
            }

            if (script_uitomouse != null) {                                     //turn off footstep sounds before loading new scene
                script_uitomouse.stopSound();
            }
        }

        public IEnumerator StartFadeOut() {                                    // this is the function that starts the fade out coroutine
            FindFadeObject();
            
            if (fadeObject != null) {
                yield return StartCoroutine(FadeOutCoroutine());
            }

            if (script_uitomouse != null) {                                     //turn off footstep sounds before loading new scene
                script_uitomouse.stopSound();
            }


        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------Enumerator Stuff------------------------------------------------------------------------
 
        private IEnumerator FadeOutCoroutine() {                        //fade out = black screen goes clear

            if (MoveScript != null)
            {
                MoveScript.InCatScene = true;
                MoveScript.DisableInput();
                MoveScript.DisableInteract();
            }
            yield return new WaitForSeconds(0.5f);                      //for the fade out, we wait a bit in the beginning before it starts
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); 

                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null; 
            }

            if (MoveScript != null)
            {
                MoveScript.InCatScene = false;
                MoveScript.Activate_CallEnableInput();
                MoveScript.Activate_CallEnableInteract();
            }

            if(GloveConnect == null && GameObject.FindGameObjectWithTag("GloveOfPower") != null)
            {
                GloveConnect = GameObject.FindGameObjectWithTag("GloveOfPower").GetComponent<GloveScript>();
            }
            if (GloveConnect != null && GloveScript.CallGlove)
            {
                MoveScript.DisableInput();
                MoveScript.DisableInteract();
                GloveScript.CallGlove = false;
                GloveConnect.ActivateGlove();
            }

            fadeImage.color = new Color(0, 0, 0, 0);
            fadeObject.SetActive(false);

        }
        //-------------------------------------------

        private IEnumerator FadeInCoroutine() {                         //fade in = screen goes black
            fadeObject.SetActive(true);
            float elapsedTime = 0f;

            if (MoveScript != null)
            {
                MoveScript.InCatScene = true;
                MoveScript.DisableInput();
                MoveScript.DisableInteract();
            }

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); 

                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null; 
            }

            if (MoveScript != null)
            {
                MoveScript.InCatScene = false;
                MoveScript.Activate_CallEnableInput();
                MoveScript.Activate_CallEnableInteract();
            }

            fadeImage.color = new Color(0, 0, 0, 1);
            
            yield return new WaitForSeconds(1f);                        //for the fade in, we wait a bit in the end before it ends
        }
    }

}
