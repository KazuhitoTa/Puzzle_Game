using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static int stageNumber = 0;
    public int buttonNumber; // �{�^���̔ԍ�

    private void Start()
    {
        Button button = GetComponent<Button>();

        // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo���֐���ݒ�
        button.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        // �{�^���̖��O���擾
        string buttonName = gameObject.name;

        // �{�^��������ԍ��𒊏o
        string buttonNumberStr = buttonName.Replace("Button", "");

        int buttonNumber;
        if (int.TryParse(buttonNumberStr, out buttonNumber))
        {
            stageNumber = buttonNumber;
            //Debug.Log(stageNumber);
            GoGame();

        }
        else
        {
            Debug.LogError("�{�^���ԍ��̎擾�Ɏ��s" + buttonName);
        }
    }

    void GoGame()
    {
        SceneManager.LoadScene("kurukuruGame");
    }
}
