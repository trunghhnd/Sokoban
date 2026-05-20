using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    public void TurnOnMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    public void TurnOffMusic()
    {
        musicSource.Stop();
    }
}
