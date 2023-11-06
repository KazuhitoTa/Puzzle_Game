using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
public class CreateButton : MonoBehaviour
{
    public int buttonCount = 0;
    public GameObject buttonPrefab; 
    public Transform canvasTransform; 
    public TMP_FontAsset textMeshProFont;
    private string fileName; // ファイル名
    private string filePath; // ファイルパス
    void Awake()
    {
        fileName = "SaveData.csv";
        filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        // ファイルが存在しない場合、新しいファイルを作成
        if (!File.Exists(filePath))
        {
            // 初期データを作成
            string initialData = "StageData,0";
            File.WriteAllText(filePath, initialData);
        }
    }
    private void Start()
    {
            for (int i = 0; i < buttonCount; i++)
            {
                GameObject button = Instantiate(buttonPrefab, canvasTransform);
                button.name = (i + 1).ToString();
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.font = textMeshProFont;
                buttonText.text = "Stage" + (i + 1).ToString();
                button.transform.position = new Vector3(0, -i, 0);
                Button buttonComponent = button.GetComponent<Button>();
                int stage;
                if (int.TryParse(ReadSpecificValueFromCSV(0, 1), out stage))
                {
                    if (i <= stage)
                    {
                        buttonComponent.interactable = true;
                    }
                    else
                    {
                        buttonComponent.interactable = false;
                    }
                }
            }
        
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
            return null;    // 返す値がない場合、nullを返す
        }
}
