using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour
{
    float speed = 3f; // 変化速度
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float alpha = Mathf.Sin((Time.time - 0.5f) * speed) * 0.5f + 0.5f; // Sin関数を使って0から1の範囲でループ
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha); // Imageのアルファ値を設定
    }
}

