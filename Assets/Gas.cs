using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;

public class Gas : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        play=GameObject.FindWithTag("GameController").GetComponent<Play>();
		gameManager=GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //hp = 30;
		dec = 10;
		inv = 2;
		//invtime = 5;
		unt = 1;
		//unttime = 3;
		ct = 5.0f;
		rt=5.0f;
		SetActPattern(20, 55, 25);
		Debug.Log(hp);
		play.EnemyInit(hp);
		 
    }

}
