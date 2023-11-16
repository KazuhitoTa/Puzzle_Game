using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public List<Map> Maps=new List<Map>();

    [System.Serializable]
    public class Map
    {
        [SerializeField]private GameObject enemyPrefab;
        public int hp;          // 体力 HitPoint
        public float dec;       // 制限時間を減少させる値 Decrease
        public int inv;     // 暗闇にする個数 Invisible
        //int invtime = 0;  // 暗闇にする時間 InvisibleTime
        public int unt;     // 操作不可の個数 Untachable
        //int unttime = 0;  // 操作不可の時間 UntachableTime
        public float ct;        // 行動のクールタイム CoolTime
        public Image image;
        
        public float rt;    // 行動の予測タイム ReadyTime
        public Vector3 actParameter;
        [SerializeField]private float time;
        
        


        public GameObject EnemyPrefab{get=>enemyPrefab;}
        public float Time{get=>time;}

    }
    
}
