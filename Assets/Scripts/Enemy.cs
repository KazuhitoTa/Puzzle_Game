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
    public int[] actPattern = new int[100]; // 行動パターン（長さ100の整数配列）
    public Play gameManager;
    // 関数 ------------------------------------------------------------------
    // 0.時間減少
    float CalcDecTime() {
        return UnityEngine.Random.Range(dec * 0.9f, dec * 1.1f);
    }
    // 1.暗闇
    int CalcInvPanel() {
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
    public void SetActPattern(int a, int b, int c) {
        for (int i = 0; i < 100; i++) {
            if (i < a) actPattern[i] = 0;
            else if (i < a + b) actPattern[i] = 1;
            else if (i < a + b + c) actPattern[i] = 2;
            else Debug.Log("行動パターンの数が不適です");
        }
    }
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
}