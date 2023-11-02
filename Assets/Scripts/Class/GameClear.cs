using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameClear : MonoBehaviour
{
    [SerializeField]List<GameObject> gameObjects=new List<GameObject>();

    string filePath = "Assets/Resources/SaveData/SaveData.csv";   // 書き込み先ファイルのパス
    public void GameClearStart()
    {
        Debug.Log("start");
        foreach (var item in gameObjects)
        {
            item.SetActive(true);
        }

        ChangeStageState(ButtonManager.stageNumber);
    }

    public void GameClearUpdate()
    {
        
    }

    public void GameClearEnd()
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
    }

        // ステージのクリア状況を変える関数
        // ChangeStageState(int ステージ番号)で行う
        // ※csvに書き込むところまで実行してある
        void ChangeStageState(int stageState)
        {
            int clearMaxStageNum = 0;
            if (int.TryParse(ReadSpecificValueFromCSV(0, 1), out clearMaxStageNum))
            {
                // StageNumberがリストの最大値より小さいかチェック
                if (stageState > clearMaxStageNum)
                {
                    Debug.Log("クリアステージを" + stageState + "に変更しました。");

                    SaveToCSV(0, 1, stageState);   // SaveData.csvに書き込み
                }
                else
                {
                    Debug.LogError("要素の変更に失敗しました");
                }
            }
        }

    // SaveData.csvの特定座標に特定の値を書き込む関数
        // SaveToCSV(int 縦座標, int 横座標, int 変えたい値)で行う
        void SaveToCSV(int writeColPlace, int writeRowPlace, int writeValue)
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
