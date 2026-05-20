using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip winMusic;
    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayWinMusic()
    {
        audioSource.Stop();

        audioSource.clip = winMusic;
        audioSource.loop = false;
        audioSource.Play();
    }
}
