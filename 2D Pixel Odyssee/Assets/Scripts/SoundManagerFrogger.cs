using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SfxSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip gameOver;
    public AudioClip jump;
    public AudioClip score;
    public AudioClip win;

    
    
    
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        SfxSource.clip = clip;
        SfxSource.Play();
    }

    public void StopMusic(AudioClip clip)
    {
        musicSource.Stop();
    }
    
}
