using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;
    public static SoundManager GetInstance() 
    {
        return Instance;
    }

    public AudioSource Fly, PipePass, Dead, Congradulations, ClickSound;
    public bool muteSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFlySound()
    {
        if (!muteSound)
            Fly.Play();
    }

    public void PlayPipePassSound() 
    {
        if (!muteSound)
            PipePass.Play();
    }

    public void PlayDeadSound()
    {
        if (!muteSound)
            Dead.Play();
    }

    public void PlayCongradulationsSound()
    {
        if (!muteSound)
            Congradulations.Play();
    }

    public void PlayClickSound() 
    {
        if (!muteSound)
            ClickSound.Play();
    }
    
}
