using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steuerung_einblenden : MonoBehaviour
{
    [SerializeField] private GameObject steuerung;
    private int einblend_counter;

    void Awake()
    {
        einblend_counter = 0;
        Einblenden();
    }

    IEnumerator Einblenden(){
        while (einblend_counter == 0){
            steuerung.SetActive(true);
            yield return new WaitForSeconds(5.0f);
            steuerung.SetActive(false);
            einblend_counter++;
        }
    }
}