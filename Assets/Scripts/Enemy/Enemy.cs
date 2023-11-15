using System;
using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;
public class Enemy :MonoBehaviour
{
    // 変数 ------------------------------------------------------------------
    public int hp = 0;          // 体力 HitPoint
    public float dec = 0;       // 制限時間を減少させる値 Decrease
    public int inv = 0;     // 暗闇にする個数 Invisible
    //int invtime = 0;  // 暗闇にする時間 InvisibleTime
    public int unt = 0;     // 操作不可の個数 Untachable
    //int unttime = 0;  // 操作不可の時間 UntachableTime
    public float ct = 0;        // 行動のクールタイム CoolTime
    
    public float rt = 0;    // 行動の予測タイム ReadyTime
    public Vector3 actParameter;
    public int[] actPattern = new int[100]; // 行動パターン（長さ100の整数配列）
    public Play play;    // ゲームマネージャー
    public GameManager gameManager;
    public GameManager.GameState gameState;
    public float waitCount = 0;      // 待機時間
    public bool countSwitch = false;    // waitCountのスイッチ
    public Animator animator;

    public MapData mapData;

    public int actcol=0;
    public Animator enemyAnimator;
	public Animator subAnimator;
    public bool waitFlog=false;
    public bool attackFlog=false;
    public bool missAtkFlog=false;
    public bool dedFlog=false;
    public bool pauseFlog=false;
    public float ctTemp;
   

    // 関数 ------------------------------------------------------------------
    // 0.時間減少
    public float CalcDecTime() {
        return UnityEngine.Random.Range(dec * 0.9f, dec * 1.1f);
    }
    // 1.暗闇
    public int CalcInvPanel() {
        int invCount = UnityEngine.Random.Range(inv - 2, inv + 2 + 1);
        if (invCount < 1) invCount = 1;
        return invCount;
    }
    // 2.操作不可
    public int CalcUntPanel() {
        int untCount = UnityEngine.Random.Range(unt - 2, unt + 2 + 1);
        if (untCount < 1) untCount = 1;
        return untCount;
    }
    // 行動パターンの割り振り
    public void SetActPattern(Vector3 a) {
        for (int i = 0; i < 100; i++) {
            if (i < a.x) actPattern[i] = 0;
            else if (i < a.x + a.y) actPattern[i] = 1;
            else if (i < a.x + a.y + a.z) actPattern[i] = 2;
            else Debug.Log("行動パターンの数が不適です");
        }
    }



    // Update関数
    protected virtual void Update()
    {
        
        if(gameManager.GetGameState()==GameManager.GameState.Play)
        {
            if(play.enemyStop())
            {
                enemyAnimator.enabled=false;
                pauseFlog=true;
            }
            else
            {
                enemyAnimator.enabled=true;
                 // waitCountを加算
                waitCount += Time.deltaTime;
            }
            

            // switch分岐
            if (!countSwitch)
            {
                // ctだけ待つ
                if (waitCount >= ctTemp)
                {
                    ctTemp=ct;
                    // 攻撃番号の設定
                    actcol = UnityEngine.Random.Range(0, 3);
                    // 攻撃予告
                    play.ActReady(actcol, rt);
                    // waitCountリセット
                    waitCount = 0;
                    // countSwitch切替
                    countSwitch = true;
                    // ct変動
                    ctTemp+=UnityEngine.Random.Range(-2,2);
                }
            }
            else 
            {
                // rtだけ待つ
                if (waitCount >= rt)
                {
                    // countSwitch切替
                    countSwitch = false;

                    // 攻撃アニメーション
                    //------------------------------
                    // ANIMATION OF ENEMY ATTACK!!!
                    //------------------------------
                    // 防御番号の判別
                    if (actcol != play.NowButtonNum())
                    {
                        attackFlog=true;
                       // 攻撃のランダム処理
                        int tmp = UnityEngine.Random.Range(0, 100);
                        switch (actPattern[tmp]) {
                            case 0:
                                Debug.Log("時間減少!!");
                                // 処理
                                play.DecTime(CalcDecTime());
                                break;
                            case 1:
                                Debug.Log("暗闇!!");
                                // 処理
                                //play.InvPanel(CalcInvPanel());
                                play.InvPanel(inv);
                                break;
                            case 2:
                                Debug.Log("操作不可!!");
                                // 処理
                                play.UntPanel(CalcUntPanel());
                                break;
                        }
                    }
                    else 
                    {
                        missAtkFlog=true;
                        play.Grd();
                    }

                    waitCount = 0;
                }
            }

        }
        else if(gameManager.GetGameState()==GameManager.GameState.GameClear)
        {
            if(!waitFlog)
            {
                enemyAnimator.enabled=true;
                subAnimator.enabled=true;
                waitFlog=true;
                dedFlog=true;
            } 
        }
        else if(gameManager.GetGameState()==GameManager.GameState.Pause)
        {
            enemyAnimator.enabled=false;
            pauseFlog=true;
        }
     
    }


    


    /*
    // コルーチンによるループ処理
    public IEnumerator ActRoutine()
    {
        while (true)
        {
            // ct秒待つ
            yield return new WaitForSeconds(ct);
            // 攻撃番号の設定
            int actcol = UnityEngine.Random.Range(0, 3);
            // 攻撃予告
            gameManager.ActReady(actcol, rt);
            // rt秒待つ
            yield return new WaitForSeconds(rt);
            // 攻撃アニメーション
            //----------------------------
            //ANIMATION OF ENEMY ATTACK!!!
            //----------------------------
            // 防御番号の判別
            if (actcol != gameManager.NowButtonNum())
            {
                // 攻撃のランダム処理
                int tmp = UnityEngine.Random.Range(0, 100);
                switch (actPattern[tmp]) {
                case 0:
                    Debug.Log("時間減少!!");
                    // 処理
                    gameManager.DecTime(CalcDecTime());
                    break;
                case 1:
                    Debug.Log("暗闇!!");
                    // 処理
                    gameManager.InvPanel(CalcInvPanel());
                    break;
                case 2:
                    Debug.Log("操作不可!!");
                    // 処理
                    gameManager.UntPanel(CalcUntPanel());
                    break;
                }
            }
            // ctの変化
            //ct += 2;
        }
    }
    */
}