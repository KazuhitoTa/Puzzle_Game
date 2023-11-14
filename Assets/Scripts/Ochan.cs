using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ochan : MonoBehaviour
{
    public void GetManager()
    {
        Play play=GameObject.FindWithTag("GameController").GetComponent<Play>();
        play.EnemyDamage();
    }
}
