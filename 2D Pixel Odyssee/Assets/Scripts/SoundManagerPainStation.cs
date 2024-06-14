using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource PSmusicSource;
    [SerializeField] AudioSource PSSfxSource;

    public AudioClip CollisionPlayer;
    public AudioClip CollisionWall;
    public AudioClip PlayerLost;
    public AudioClip PlayerWin;
    public AudioClip PointEnemy;
    public AudioClip PointPlayer;
    public AudioClip MusicPainStation;

    
    
    
    public void PlayMusicPS(AudioClip clip)
    {
        PSmusicSource.clip = MusicPainStation;
        PSmusicSource.Play();
    }

    public void PlaySfxPS(AudioClip clip)
    {
        PSSfxSource.clip = clip;
        PSSfxSource.Play();
    }

    public void StopSfxPS(AudioClip clip)
    {
        PSmusicSource.Stop();
    }
    public void StopMusicPS(AudioClip clip)
    {
        PSmusicSource.Stop();
    }
    
}
