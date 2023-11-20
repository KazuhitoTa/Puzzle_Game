using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ochan : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;
    [SerializeField]List<AudioClip> audioClips=new();
    public void GetManager()
    {
        Play play=GameObject.FindWithTag("GameController").GetComponent<Play>();
        play.EnemyDamage();
    }

    public void SoundOchan()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}
