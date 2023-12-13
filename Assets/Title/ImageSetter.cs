using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSetter : MonoBehaviour
{
    public Sprite[] imageOptions; // ランダムに選択する画像の配列

    void Start()
    {
        SetRandomImage();
    }

    void SetRandomImage()
    {
        Image imageComponent = GetComponent<Image>();

        if (imageComponent != null && imageOptions != null && imageOptions.Length > 0)
        {
            // ランダムなインデックスを選択
            int randomIndex = Random.Range(0, imageOptions.Length);

            // ランダムに選択した画像をセット
            imageComponent.sprite = imageOptions[randomIndex];
        }
        else
        {
            Debug.LogError("Image component or image options are not set properly.");
        }
    }
}

