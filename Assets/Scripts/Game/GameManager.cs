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
        [SerializeField]private List<bool> startCheck=new List<bool>();

        


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
            for(int i=0;i<Enum.GetValues(typeof(GameState)).Length;i++)
            {
                startCheck.Add(false);
            }
            play.PlayStart();
            ChangeGameState(GameState.Ready);
            
        }

        void Update()
        {
            if(!play.BGMCheck()&&play.startCheck())play.BGMPlay();
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

    }
}