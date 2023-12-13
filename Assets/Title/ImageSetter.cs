using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSetter : MonoBehaviour
{
    public Sprite[] imageOptions; // �����_���ɑI������摜�̔z��

    void Start()
    {
        SetRandomImage();
    }

    void SetRandomImage()
    {
        Image imageComponent = GetComponent<Image>();

        if (imageComponent != null && imageOptions != null && imageOptions.Length > 0)
        {
            // �����_���ȃC���f�b�N�X��I��
            int randomIndex = Random.Range(0, imageOptions.Length);

            // �����_���ɑI�������摜���Z�b�g
            imageComponent.sprite = imageOptions[randomIndex];
        }
        else
        {
            Debug.LogError("Image component or image options are not set properly.");
        }
    }
}

