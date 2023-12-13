using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class TitleManager : MonoBehaviour
{
    private string fileName;
    private string filePath;
    string[] initialData = { "StageData,0", "SelectData,0", "BGM,1", "SE,1", "tutorial,false" };
    string Data;

    void Awake()
     {
         fileName = "SaveData.csv";
         filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);

        //// csvファイルが無かったら
        //if (!File.Exists(filePath))
        //{
        //    File.WriteAllLines(filePath, initialData);
        //}
        //else
        //{
            if ((Data = ReadSpecificValueFromCSV(0, 1)) != "")
            {
                initialData[0] = "StageData," + Data;
            }
            if ((Data = ReadSpecificValueFromCSV(1, 1)) != "")
            {
                initialData[1] = "SelectData," + Data;
            }
            if ((Data = ReadSpecificValueFromCSV(2, 1)) != "")
            {
                initialData[2] = "BGM," + Data;
            }
            if ((Data = ReadSpecificValueFromCSV(3, 1)) != "")
            {
                initialData[3] = "SE," + Data;
            }
            if ((Data = ReadSpecificValueFromCSV(4, 1)) != "")
            {
                initialData[4] = "tutorial," + Data;
            }

            File.WriteAllLines(filePath, initialData);

        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SaveData.csvの指定された位置のCSVファイル内の値を取得する関数
    // ReadSpecificValueFromCSV(int 縦座標, int 横座標)で行う
    string ReadSpecificValueFromCSV(int roadColPlace, int roadRowPlace)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);   // 行ごとに読み込み

            // 列の座標が配列の長さ未満かどうかを確認
            if (roadColPlace < lines.Length)
            {
                string[] elements = lines[roadColPlace].Split(','); // 行の座標に対応する行を取得しカンマで分割して配列elementsに格納

                // 行の座標が配列の長さ未満かどうかを確認
                if (roadRowPlace < elements.Length)
                {
                    return elements[roadRowPlace];  //stringで値を返す
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
            Debug.LogError("CSVファイルの読み込み中にエラーが発生しました: " + e.Message);
        }

        return "";    // 返す値がない場合、空白を返す
    }

    public void GoStageSelect()
    {
        SceneManager.LoadScene("Stage");
    }
}
