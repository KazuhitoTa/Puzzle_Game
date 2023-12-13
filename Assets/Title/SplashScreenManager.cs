using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SplashScreenManager : MonoBehaviour
{
    float speed = 1f; // �ω����x
    float T;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        T = 0;
    }

    void Update()
    {
        Splash();
    }

    void Splash()
    {
        T += Time.deltaTime;
        float alpha = Mathf.Sin(T * speed); // Sin�֐����g����0����1�͈̔͂Ń��[�v
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha); // Image�̃A���t�@�l��ݒ�

        if(alpha < 0)
        {
            SceneManager.LoadScene("Title");
        }
    }


}
