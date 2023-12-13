using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class SetUp : MonoBehaviour
{
    [SerializeField]GameObject gameObject;
    [SerializeField]Slider BGMSlider;
    [SerializeField]Slider SESlider;
    [SerializeField]AudioSource BGMAudioSource;
    [SerializeField]AudioSource SEAudioSource;

    [SerializeField]List<GameObject> tutorialObjects=new List<GameObject>();

    private string fileName;
    private string filePath;
    // Start is called before the first frame update
    void Start()
    {
        fileName = "SaveData.csv";
        filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        BGMAudioSource.Play();
        BGMSlider.value=TitleBGM.BGMVolume;
        BGMAudioSource.volume=TitleBGM.BGMVolume;
        SESlider.value=TitleBGM.SEVolume;
        SEAudioSource.volume=TitleBGM.SEVolume;
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
        TitleBGM.BGMVolume=volume;
        Debug.Log(volume);
        SaveToCSV(2,1,volume);
    }
    
    public void SEVolume(float volume)
    {
        SEAudioSource.volume =volume;
        TitleBGM.SEVolume=volume;
        Debug.Log(volume);
        SaveToCSV(3,1,volume);
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }


    public void SEPlay()
    {
        SEAudioSource.Play();
    }

    public void tutorialPop()
    {
        tutorialObjects[0].SetActive(true);
    }

    public void tutorial01()
    {
        tutorialObjects[0].SetActive(false);
        tutorialObjects[1].SetActive(true);
    }

    public void tutorialEnd()
    {
        tutorialObjects[0].SetActive(false);
        tutorialObjects[1].SetActive(false);
    }


    void SaveToCSV(int writeColPlace, int writeRowPlace, float writeValue)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);   // 列ごとに読み込み
            if (writeColPlace < lines.Length)               // 列の座標が配列の長さ未満かどうかを確認
            {
                string[] elements = lines[writeColPlace].Split(',');  // 行の座標に対応する行を取得しカンマで分割して配列elementsに格納
                if (writeRowPlace < elements.Length)                  // 行の座標が配列の長さ未満かどうかを確認
                {
                    elements[writeRowPlace] = writeValue.ToString();      // 文字列に変換し、elements内の指定された列の座標の要素を変更
                    lines[writeColPlace] = string.Join(",", elements);    // elementsを再びカンマで連結（しないといけないらしい）
                    File.WriteAllLines(filePath, lines);                // 更新されたlinesを書き込み
                }
                else
                {
                    Debug.LogError("指定された行が範囲外です");
                }
            }
            else
            {
                Debug.LogError("指定された列が範囲外です");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("書き込み失敗" + e.Message);
        }
    }
}
