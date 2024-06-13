using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource SWmusicSource;
    [SerializeField] AudioSource SWSfxSource;

    public AudioClip musicSW;
    public AudioClip BoostSW;
    public AudioClip gameOverSW;
    public AudioClip GotHitSW;
    public AudioClip HitEnemySW;
    public AudioClip winSW;
    public AudioClip ShootSW;

    
    
    
    public void PlayMusicSW(AudioClip clip)
    {
        SWmusicSource.clip = musicSW;
        SWmusicSource.Play();
    }

    public void PlaySfxSW(AudioClip clip)
    {
        SWSfxSource.clip = clip;
        SWSfxSource.Play();
    }

    public void StopMusicSW(AudioClip clip)
    {
        SWmusicSource.Stop();
    }
    
}
