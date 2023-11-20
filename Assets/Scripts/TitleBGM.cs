using UnityEngine;
using System.Collections.Generic;

public class TitleBGM : MonoBehaviour
{
    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] AudioSource SEAudioSource;
    [SerializeField] List<AudioClip> BGMAudioClips = new List<AudioClip>();


    public static float volume;

    private bool hasPlayedFirstClip = false;

    private void Start()
    {
        volume=0.5f;
        BGMAudioSource.volume=volume;
        if (BGMAudioClips.Count >= 2 && BGMAudioSource != null)
        {
            // 最初のクリップを再生
            PlayFirstClip();
        }
        else
        {
            Debug.LogError("Audio clips or AudioSource not properly set!");
        }
    }

    private void Update()
    {
        // 最初のクリップが再生された後、それ以降は2番目のクリップをループ再生する
        if (!BGMAudioSource.isPlaying&&hasPlayedFirstClip)
        {
            BGMAudioSource.clip = BGMAudioClips[1]; // 2番目のクリップを設定
            BGMAudioSource.loop = true; // ループ再生を有効にする
            BGMAudioSource.Play(); // 2番目のクリップを再生
        }
    }

    private void PlayFirstClip()
    {
        BGMAudioSource.clip = BGMAudioClips[0]; // 1番目のクリップを設定
        BGMAudioSource.Play(); // 1番目のクリップを再生
        hasPlayedFirstClip=true;
    }
    public void SEPlay()
    {
        SEAudioSource.Play();
    }
}
