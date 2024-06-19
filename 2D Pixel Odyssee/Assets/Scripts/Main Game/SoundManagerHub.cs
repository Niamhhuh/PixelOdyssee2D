using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManagerHub : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SfxSource;

    public AudioClip HubMusic;
    public AudioClip WalkingBebe;
    public AudioClip WalkingRosie;
    public AudioClip Dialog;
    public AudioClip ItemPickUp;
    public AudioClip OpenDoor;
    public AudioClip OpenInventar;
    public AudioClip RosiePush;




    public void PlayMusicHub(AudioClip clip)
    {
        musicSource.clip = HubMusic;
        musicSource.Play();
    }

    public void PlaySfxHub(AudioClip clip)
    {
        SfxSource.clip = clip;
        SfxSource.Play();
    }

    public void StopMusicHub(AudioClip clip)
    {
        musicSource.Stop();
    }

    public void StopSfxHub(AudioClip clip)
    {
        SfxSource.Stop();
    }

}
