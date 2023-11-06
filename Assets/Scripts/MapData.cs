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
        [SerializeField]private GameObject playerPrefab;
        [SerializeField]private Image backGround;
        [SerializeField]private int enemyHP;
        [SerializeField] private int enemyAtk;


        public GameObject EnemyPrefab{get=>enemyPrefab;}
        public GameObject PlayerPrefab{get=>playerPrefab;}
        public Image BackGround{get=>backGround;}
        public int EnemyHP{get=>enemyHP;}
        public int EnemyAtk{get=>enemyAtk;}
    }
    
}
