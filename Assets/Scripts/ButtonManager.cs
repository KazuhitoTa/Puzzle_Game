using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static int stageNumber = 0;
    public int buttonNumber;

    private void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        string buttonName = gameObject.name;

        string buttonNumberStr = buttonName.Replace("Button", "");

        int buttonNumber;
        if (int.TryParse(buttonNumberStr, out buttonNumber))
        {
            stageNumber = buttonNumber;
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
