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
        [SerializeField]GameClear gameClear;
        [SerializeField]GameOver gameOver;
        public GameState gameState=GameState.Play;
        [SerializeField]private List<bool> startCheck=new List<bool>();


        public enum GameState
        {
            Play,
            Pause,
            GameClear,
            GameOver
        }

        

        void Start()
        {
            for(int i=0;i<Enum.GetValues(typeof(GameState)).Length;i++)
            {
                startCheck.Add(false);
            }
            play.PlayStart();
        }

        void Update()
        {
            switch (gameState)
            {
                case GameState.Play:
                    // プレイ中の処理を実行
                    play.PlayUpdate();
                    break;

                case GameState.Pause:
                    // ポーズ中の処理を実行
                    pause.PauseUpdate();
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
            Debug.Log("end");
            
            if (gameStateTemp==GameState.Pause)     pause.PauseStart();
            else if (gameStateTemp==GameState.GameClear) gameClear.GameClearStart();
            else if (gameStateTemp==GameState.GameOver)  gameOver.GameOverStart();
            else if (gameStateTemp==GameState.Play) play.PlayInit();
            gameState=gameStateTemp;
        }

        void ResetGameStart(int StateNum)
        {
            for(int i=0;i<Enum.GetValues(typeof(GameState)).Length;i++)
            {
                if(i==StateNum)startCheck[i]=true;
                else startCheck[i]=false;
            }
        }

        public void GoStageSelect()
        {
            SceneManager.LoadScene("StageSelect");
        }
        public void GoGame()
        {
            SceneManager.LoadScene("kurukuruGame");
        }

        public GameManager.GameState GetGameState()
        {
            return gameState;
        }

    }
}