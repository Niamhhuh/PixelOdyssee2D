using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerHub : MonoBehaviour
{
    SoundManagerHub SoundManagerHub;
    void Start()
    {
        SoundManagerHub = GameObject.FindGameObjectWithTag("SoundManagerHub").GetComponent<SoundManagerHub>();
        SoundManagerHub.PlayMusicHub(SoundManagerHub.HubMusic);
    }

}
