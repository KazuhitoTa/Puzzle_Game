using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetUp : MonoBehaviour
{
    [SerializeField]GameObject gameObject;
    [SerializeField]Slider BGMSlider;
    [SerializeField]AudioSource BGMAudioSource;
    [SerializeField]AudioSource SEAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        BGMAudioSource.Play();
        BGMSlider.value=TitleBGM.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopUp()
    {
        gameObject.SetActive(true);
    }

    public void PopUpExit()
    {
        gameObject.SetActive(false);
    }

    public void SoundVolume(float volume)
    {
        BGMAudioSource.volume =volume;
        TitleBGM.volume=volume;
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void SESoundVolume(float volume)
    {
        SEAudioSource.volume =volume;
    }

    public void SEPlay()
    {
        SEAudioSource.Play();
    }
}
