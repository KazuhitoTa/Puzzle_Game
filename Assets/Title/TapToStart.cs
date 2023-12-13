using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour
{
    float speed = 3f; // �ω����x
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float alpha = Mathf.Sin((Time.time - 0.5f) * speed) * 0.5f + 0.5f; // Sin�֐����g����0����1�͈̔͂Ń��[�v
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha); // Image�̃A���t�@�l��ݒ�
    }
}

