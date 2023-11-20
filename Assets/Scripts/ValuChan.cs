using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuChan : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips=new();
    
    public void SoundValChan()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
