using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClips : MonoBehaviour
{
    public AudioClip  Flick; 
    public AudioClip  UISelector; 
    public AudioClip  Back; 
    public AudioClip  Confirm; 
    public AudioSource AudioSource; 

    void Start()
    {
        
    }

    public void PlayUISound()
    {
        AudioSource.clip = UISelector; 
        AudioSource.Play();

    }

    public void PlayBack()
    {
        AudioSource.clip = Back; 
        AudioSource.Play();
    }
    public void PlayFlick()
    {
        AudioSource.clip = Flick;
        AudioSource.Play(); 
    }
    public void PlayConfirm()
    {
        AudioSource.clip = Confirm;
        AudioSource.Play(); 
    }
    
}
