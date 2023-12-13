using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSplash : MonoBehaviour
{
    float speed = 0.7f; // �ω����x
    private Image image;
    static bool splash = false;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!splash)
        {
            Splash();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Splash()
    {
        float alpha = Mathf.Sin((Time.time+0.5f) * speed); // Sin�֐����g����0����1�͈̔͂Ń��[�v
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha); // Image�̃A���t�@�l��ݒ�

        if (alpha < 0)
        {
            Destroy(gameObject);
            splash = true;
        }
    }


}
