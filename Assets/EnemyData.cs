using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public List<Enemy> enemies=new List<Enemy>();

    [System.Serializable]
    public class Enemy
    {
        [SerializeField]private GameObject enemyPrefab;
        [SerializeField]private string name;
        [SerializeField, TextArea(1,5)]private string information;
        [SerializeField]private int hP;
        [SerializeField] private int atk;

        public GameObject EnemyPrefab{get=>enemyPrefab;}
        public string Name{get=>name;}
        public int HP{get=>hP;}
        public int Atk{get=>atk;}
    }
    
}
