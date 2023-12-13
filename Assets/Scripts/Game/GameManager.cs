using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


namespace kurukuru
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]Play play;
        [SerializeField]Pause pause;
        [SerializeField]Tutorial tutorial;
        [SerializeField]GameClear gameClear;
        [SerializeField]GameOver gameOver;
        [SerializeField]Ready ready;
        public GameState gameState=GameState.Play;
       

        private string fileName;
        private string filePath;

        public enum GameState
        {
            Ready,
            Play,
            Pause,
            Tutorial,
            GameClear,
            GameOver
        }

        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        

        void Start()
        {
            fileName="SaveData.csv";
            filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            play.PlayStart();
            
            if(ReadSpecificValueFromCSV(4, 1)=="false")
            {
                ChangeGameState(GameState.Tutorial);
                SaveToCSV(4,1,"true");
            }
            else ChangeGameState(GameState.Ready);
        }

        void Update()
        {
            if(!play.BGMCheck()&&play.startCheck()&&gameState!=GameState.GameClear)play.BGMPlay();
            Debug.Log(gameState);
            switch (gameState)
            {
                case GameState.Ready:
                    //プレイ前の待ち時間の処理を書く
                    break;

                case GameState.Play:
                    // プレイ中の処理を実行
                    play.PlayUpdate();
                    break;

                case GameState.Pause:
                    // ポーズ中の処理を実行
                    pause.PauseUpdate();
                    break;

                case GameState.Tutorial:
                    //チュートリアル中の処理を実行
                    tutorial.TutorialUpdate();
                    break;

                case GameState.GameClear:
                    // ゲームクリア時の処理を実行
                    gameClear.GameClearUpdate();
                    break;

                case GameState.GameOver:
                    // ゲームオーバー時の処理を実行
                    gameOver.GameOverUpdate();
                    break;

                default:
                    // 未知の状態に対する処理を実行
                    break;
            }
        }

        public void ChangeGameState(GameState gameStateTemp)
        {   
            if (gameStateTemp==GameState.Pause)     pause.PauseStart();
            else if (gameStateTemp==GameState.GameClear) gameClear.GameClearStart();
            else if (gameStateTemp==GameState.GameOver)  gameOver.GameOverStart();
            else if(gameStateTemp==GameState.Tutorial) tutorial.TutorialStart();
            else if (gameStateTemp==GameState.Play) play.PlayInit();
            else if( gameStateTemp==GameState.Ready)ready.ReadyStart();
            gameState=gameStateTemp;
        }


        public void GoStageSelect()
        {
            SceneManager.LoadScene("Stage");
        }
        public void GoGame()
        {
            SceneManager.LoadScene("kurukuruGame");
        }

        public GameManager.GameState GetGameState()
        {
            return gameState;
        }

        string ReadSpecificValueFromCSV(int roadColPlace, int roadRowPlace)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);   // �s���Ƃɓǂݍ���
                                                                // ��̍��W���z��̒����������ǂ������m�F
                if (roadColPlace < lines.Length)
                {
                    string[] elements = lines[roadColPlace].Split(','); // �s�̍��W�ɑΉ�����s���擾���J���}�ŕ������Ĕz��elements�Ɋi�[
                                                                        // �s�̍��W���z��̒����������ǂ������m�F
                    if (roadRowPlace < elements.Length)
                    {
                        return elements[roadRowPlace];  //string�Œl��Ԃ�
                    }
                    else
                    {
                        Debug.LogError("�w�肳�ꂽ�s���͈͊O�ł�");
                    }
                }
                else
                {
                    Debug.LogError("�w�肳�ꂽ�񂪔͈͊O�ł�");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("CSV�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: " + e.Message);
            }
            return null;    // �Ԃ��l���Ȃ��ꍇ�Anull��Ԃ�
        }  

        void SaveToCSV(int writeColPlace, int writeRowPlace, string writeValue)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);   // 列ごとに読み込み
                if (writeColPlace < lines.Length)               // 列の座標が配列の長さ未満かどうかを確認
                {
                    string[] elements = lines[writeColPlace].Split(',');  // 行の座標に対応する行を取得しカンマで分割して配列elementsに格納
                    if (writeRowPlace < elements.Length)                  // 行の座標が配列の長さ未満かどうかを確認
                    {
                        elements[writeRowPlace] = writeValue;      // 文字列に変換し、elements内の指定された列の座標の要素を変更
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
}